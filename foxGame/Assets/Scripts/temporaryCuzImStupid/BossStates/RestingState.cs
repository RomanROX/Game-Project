using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestingState : StateMachineBehaviour
{
    BossBase bossBase;
    Rigidbody2D rb;
    [SerializeField] float delayTillNextState;
    Animator animator;

    Vector2 spot;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossBase = animator.gameObject.GetComponent<BossBase>();
        rb = animator.gameObject.GetComponent<Rigidbody2D>();
        this.animator = animator;
        spot = bossBase.GetRandomRestingSpot();
        Move();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Move();
        if (CheckDistance())
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
        return dis < 0.01f;
    }
    void Move()
    {
        Debug.Log("should move");
        Vector2 currentPos = bossBase.transform.position;

        Vector2 final = Vector2.MoveTowards(currentPos, spot, bossBase.Speed*Time.fixedDeltaTime);
        rb.MovePosition(final);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
