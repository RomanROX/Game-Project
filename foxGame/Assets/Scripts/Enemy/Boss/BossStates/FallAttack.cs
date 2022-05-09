using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallAttack : StateMachineBehaviour
{
    Transform player;
    BossBase bossBase;
    Rigidbody2D rb;
    [SerializeField] float delayTillNextState;

    Vector2 spot;
    float speed;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossBase = animator.gameObject.GetComponent<BossBase>();
        rb = animator.gameObject.GetComponent<Rigidbody2D>();

        player = bossBase.PlayerObj.transform;
        spot = player.position;
        speed = bossBase.FallSpeed;
        rb.gravityScale = bossBase.GravityScale;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Move();
        if (CheckDistance())
            animator.SetTrigger("Stunned");
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Stunned");
    }

    bool CheckDistance()
    {
        float dis = Vector2.Distance((Vector2)bossBase.transform.position, spot);
        return dis < 2;
    }
    void Move()
    {
        //Debug.Log("should move");
        Vector2 currentPos = bossBase.transform.position;

        Vector2 final = Vector2.MoveTowards(currentPos, spot, speed*Time.fixedDeltaTime);
        rb.MovePosition(final);
    }

}
