using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : StateMachineBehaviour
{
    Transform player;
    BossBase bossBase;
    Rigidbody2D rb;
    [SerializeField] float dashDuration;


    Vector2 spot;
    float speed;
    float dashTimer;
    [SerializeField] float dashStopTime;

    public float maxDistance = 1.5f;

    

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossBase = animator.gameObject.GetComponent<BossBase>();
        rb = animator.gameObject.GetComponent<Rigidbody2D>();

        player = bossBase.PlayerObj.transform;
        spot = player.position;
        speed = bossBase.DashSpeed;
        rb.gravityScale = 0;
        dashTimer = dashDuration;

        DashStart();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        dashTimer-=Time.fixedDeltaTime;

        if (dashTimer < 0)
        {
            DashStop();
            animator.SetInteger("DashNum", animator.GetInteger("DashNum") + 1);
            animator.SetTrigger("FlyUp");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("FlyUp");
    }

    void DashStart()
    {
        rb.velocity = Vector2.right*DashDirection()*speed;
    }
    void DashStop()
    {
        rb.velocity = Vector2.zero;
    }
    float DashDirection()
    {
        return (spot.x-rb.transform.position.x > 0)? 1: -1;
    }

}
