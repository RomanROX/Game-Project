using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestingState : StateMachineBehaviour
{
    BossBase bossBase;
    Rigidbody2D rb;

    Vector2 spot;
    float timer;
    [SerializeField] float delayTillNextState;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossBase = animator.gameObject.GetComponent<BossBase>();
        rb = animator.gameObject.GetComponent<Rigidbody2D>();
        spot = bossBase.GetRandomRestingSpot();

        rb.gravityScale = 0;
        timer = delayTillNextState;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Move();
        if (CheckDistance())
            timer -= Time.fixedDeltaTime;
        if (timer < 0) 
            animator.SetTrigger("FallStart");
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("FallStart");
    }

    bool CheckDistance()
    {
        float dis = Vector2.Distance((Vector2)bossBase.transform.position, spot);
        return dis < 0.5f;
    }
    void Move()
    {
        //Debug.Log("should move");
        Vector2 currentPos = bossBase.transform.position;

        Vector2 final = Vector2.MoveTowards(currentPos, spot, bossBase.Speed*Time.fixedDeltaTime);
        rb.MovePosition(final);
    }

}
