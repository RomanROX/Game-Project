using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum BossStates
{
    charging,
    dashing,
    stunned,
    throwing,
    jumping,
    jumpAttacking
}

public class BossBehaviour : MonoBehaviour
{
    LayerMask player_ ;
    Rigidbody2D rb;
    BossStates state;

    [SerializeField] GameObject player;
    [SerializeField] LayerMask ground;
    [SerializeField] Vector2 colliderSize;

    bool hasHitPlayer;
    bool hasHitGround;
    bool isVunrelable;
    bool isDashing;
    bool isDashAttacking;
    int chargeDir;
    float chargeYLevel;
    float dashStartTime;

    [SerializeField] float dashDuration;
    [SerializeField] float dashChargeDelay;
    [SerializeField] float dashSpeed;
    [SerializeField] float speed;
    [SerializeField] float gravityScale;
    [SerializeField] float frictionAmount;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player_ = LayerHolder.Instance.Player;
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Charge());
    }
    private void Update()
    {
        //dashTimer -= Time.deltaTime;

        //if (DashAttackOver())
        //{
        //    Debug.Log("dash over");
        //    if (state == BossStates.dashing)
        //    {
        //        state = BossStates.charging;
        //        StopDash();
        //    }
        //    else if (dashTimer<0)
        //        state = BossStates.charging;
        //    //PlayerState_ = PlayerState.Idle;
        //}

        //if (state == BossStates.charging && dashTimer < 0)
        //    StartCoroutine(Charge());

        if (!isDashing)
        {
            rb.gravityScale = gravityScale;
        }

        
        

        if (DashAttackOver())
        {
            Debug.Log("dash over");
            if (isDashAttacking)
            {
                isDashAttacking = false;
                StopDash();
            }
            else if (Time.time - dashStartTime > dashDuration + 0.1f)
                isDashing = false;


            //PlayerState_ = PlayerState.Idle;
        }
        if (Mathf.Abs(rb.velocity.x) > 0)
            Friction();

    }

    IEnumerator Charge()
    {
        Debug.Log("charging");
        SetPlayerDir();
        yield return new WaitForSeconds(dashChargeDelay);
        Debug.Log("charged");
        while (transform.position.y != chargeYLevel)
            rb.velocity = new Vector2(-chargeDir * dashSpeed, 1);
        StartDash();
    }
    void StartDash()
    {
        Debug.Log("started dash");
        dashStartTime = Time.time;
        isDashAttacking = true;
        isDashing = true;

        rb.gravityScale = 0;

        
       // while (transform.position.y != chargeYLevel)
           // rb.velocity = new Vector2(-chargeDir * dashSpeed, 1);
        
        rb.velocity = Vector2.left* chargeDir * dashSpeed;
    }
    void StopDash()
    {
        Debug.Log("stopped dash");
        rb.gravityScale = gravityScale;
    }

    bool DashAttackOver()
    {
        return isDashing && Time.time - dashStartTime > dashDuration;
    }
    void SetPlayerDir()
    {
        chargeYLevel = player.transform.position.y;
        chargeDir = (int)Mathf.Sign(transform.position.x - player.transform.position.x);
    }

    void Friction()
    {
        rb.AddForce(Vector2.left * frictionAmount * rb.velocity.x);
    }

}
