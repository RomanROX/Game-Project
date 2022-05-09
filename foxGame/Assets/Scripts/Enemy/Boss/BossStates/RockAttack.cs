using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockAttack : StateMachineBehaviour
{
    BossBase bossBase;
    Rigidbody2D rb;

    Vector2 spot;
    public float timer;
    [SerializeField] float delayTillNextState;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossBase = animator.gameObject.GetComponent<BossBase>();
        rb = animator.gameObject.GetComponent<Rigidbody2D>();
        spot = bossBase.GetRandomRestingSpot();

        timer = delayTillNextState;
        bossBase.StartRockAttacking();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer -= Time.fixedDeltaTime;
        if (timer < 0)
        {
            bossBase.StopRockAttacking();
            animator.SetTrigger("Restart");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Restart");
    }

}
