using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum PlayerState
{
    Idle,
    Walking,
    Jumping,
    Falling,
    WallJumping,
    Dashing,
    Attacking
}


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Vector2 groundCheckRadious;
    [SerializeField] Vector2 wallCheckRadious;
    
    [SerializeField] Transform feetPos;
    [SerializeField] Transform leftWallPos;
    [SerializeField] Transform rightWallPos;

    PlayerData data;
    Rigidbody2D rb;
    Animator anim;

    float horizontal;
    float lastDir;
    float currentDir;
    Vector2 lastDashDir;

    float lastOnGroundTime;
    
    float lastOnWallRightTime;
    float lastOnWallLeftTime;
    float lastOnWallTime;
    
    float lastPressedJumpTime;
    float lastPressedDashTime;
    
    
    float wallJumpStartTime;
    float lastWallJumpDir;
    
    float dashStartTime;

    bool IsRight = true;
    bool isJumping;
    bool isWallJumping;
    bool isDashing;
    bool dashAttacking;
   
    int dashesLeft;
    int jumpsLeft;



    PlayerState PlayerState_;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        data = GameManager.Instance.PlayerData;
    }

    public void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        #region TIMERS
        lastOnGroundTime -= Time.deltaTime;
        lastOnWallLeftTime -= Time.deltaTime;
        lastOnWallRightTime -= Time.deltaTime;
        lastOnWallTime -= Time.deltaTime;

        lastPressedJumpTime -= Time.deltaTime;
        lastPressedDashTime-=Time.deltaTime;
        #endregion

        if (!isJumping && !isDashing)
        {
            if (Physics2D.OverlapBox(feetPos.position, groundCheckRadious, 0f, LayerHolder.Instance.Ground))
                lastOnGroundTime = data.coyoteTime;

            if (Physics2D.OverlapBox(leftWallPos.position, wallCheckRadious, 0f, LayerHolder.Instance.Ground) && !IsRight)
                lastOnWallLeftTime = data.coyoteTime;

            if (Physics2D.OverlapBox(rightWallPos.position, wallCheckRadious, 0f, LayerHolder.Instance.Ground) & IsRight)
                lastOnWallRightTime = data.coyoteTime;
            lastOnWallTime = Mathf.Max(lastOnWallRightTime, lastOnWallLeftTime);
        }
        #region JUMP & WALL JUMP
        if (isJumping && rb.velocity.y <= 0)
            isJumping = false;

        if (isWallJumping && Time.time - wallJumpStartTime > data.wallJumpTime)
            isWallJumping = false;

        if (!isDashing)
        {
            if (CanWallJump() && lastPressedJumpTime > 0)
            {
                //Debug.Log("wall jump?");
                isWallJumping = true;
                isJumping = false;
                wallJumpStartTime = Time.time;
                lastWallJumpDir = (lastOnWallRightTime > 0) ? -1 : 1;

                WallJump();
            }
            else if (CanJump() && lastPressedJumpTime > 0)
            {
                isJumping = true;
                isWallJumping = false;
                Jump();
            }
        }
        #endregion

        if (!isDashing)
        {
            if (rb.velocity.y >= 0)
                rb.gravityScale = data.gravityScale;
            else
                rb.gravityScale = data.fallingGravityScale;
        }

        if (DashAttackOver())
        {
            Debug.Log("dash over");
            if (dashAttacking)
            {
                dashAttacking = false;
                StopDash(lastDashDir);
            }
            else if (Time.time - dashStartTime > data.dashAttackTime + data.dashEndTime)
                isDashing = false;
            //PlayerState_ = PlayerState.Idle;
        }

        if (CanDash() && lastPressedDashTime>0)
        {
            Debug.Log("can dash");
            if (horizontal != 0)
                lastDashDir = Vector2.right * horizontal;
            else
                lastDashDir = IsRight ? Vector2.right : Vector2.left;

            dashStartTime = Time.time;
            dashesLeft--;
            dashAttacking = true;

            isDashing=true;
            isJumping = false;
            isWallJumping = false;

            StartDash(lastDashDir);
        }

        InputCallbacks();


        animate();


        //Debug.Log("Playerstate: "+PlayerState_);


    }

    private void FixedUpdate()
    {
        Move();
        if (rb.velocity.y <= 0)
        {
            //rb.velocity = new Vector2(rb.velocity.x, -fallingGravityScale);
            rb.AddForce(new Vector2(0, -data.fallingGravityScale), ForceMode2D.Force);
            PlayerState_ = PlayerState.Falling;
        }
    }


    private void InputCallbacks()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            lastPressedJumpTime = data.jumpBufferTime;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("pressed dash");
            lastPressedDashTime = data.dashBufferTime;
        }
    }


    private bool CanJump()
    {
        if (lastOnGroundTime > 0 && jumpsLeft < data.jumpAmount)
            jumpsLeft = data.jumpAmount;

        return jumpsLeft > 0 /*&& lastOnWallTime <=0*/ ;
    }
    private bool CanWallJump()
    {
        return lastPressedJumpTime > 0 && lastOnWallTime > 0 && lastOnGroundTime <= 0 &&
            (!isWallJumping || (lastOnWallRightTime > 0 && lastWallJumpDir == -1) ||
            (lastOnWallLeftTime > 0 && lastWallJumpDir == 1)) && data.isWallJumpUnlocked;
    }
    bool CanDash()
    {
        if (dashesLeft < data.dashAmount && lastOnGroundTime > 0)
            dashesLeft = data.dashAmount;
        //Debug.Log("(return) can dash? " + (dashesLeft > 0));
        return dashesLeft > 0 && data.isDashUnlocked;
    }
    bool DashAttackOver()
    {
        //bool temp = isDashing && Time.time - dashStartTime > dashAttackTime;
        //Debug.Log("(return) dash is over? "+temp);
        
        return isDashing && Time.time - dashStartTime > data.dashAttackTime;
    }

    private void Jump()
    {
        PlayerState_ = PlayerState.Jumping;
        lastPressedJumpTime = 0;
        lastOnGroundTime = 0;
        jumpsLeft--;
        if (lastOnGroundTime>0)
            rb.AddForce(Vector2.up * data.jumpSpeed * 1.5f, ForceMode2D.Impulse);
        else
            rb.AddForce(Vector2.up * data.jumpSpeed, ForceMode2D.Impulse);
    }
    private void WallJump()
    {
        PlayerState_ = PlayerState.Jumping;
        lastPressedJumpTime = 0;
        lastOnGroundTime = 0;
        lastOnWallLeftTime = 0;
        lastOnWallRightTime = 0;

        Vector2 force = data.wallJumpForce;
        force.x *= lastWallJumpDir;

        if (rb.velocity.y < 0)
            force.y -= rb.velocity.y;
        //Debug.Log(force);

        rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void Move()
    {
        
        float speedX = data.speed * horizontal;
        float force = speedX - rb.velocity.x;

        float accelRate;
        if (lastOnGroundTime > 0)
            accelRate = (Mathf.Abs(speedX) > 0.01f) ? data.runAccel : data.runDeccel;
        else
            accelRate = data.airAccel;

        float final = force * accelRate;
        rb.AddForce(Vector2.right * final);
    }

    void StartDash(Vector2 dir)
    {
        //Debug.Log("should start dash");
        lastOnGroundTime = 0;
        lastPressedDashTime = 0;

        rb.gravityScale = 0;
        rb.velocity = dir.normalized * data.dashSpeed;
    }
    void StopDash(Vector2 dir)
    {
        //Debug.Log("should stop dash");
        rb.gravityScale = data.gravityScale;

        if (dir.y>0)
        {

            rb.AddForce(Vector2.down * rb.velocity.y, ForceMode2D.Impulse);
        }
    }

    private void animate()
    {

        if (rb.velocity.x>0) { IsRight = true; }
        else if (rb.velocity.x<0) { IsRight = false; }

        
        if (rb.velocity.x!=0 && !isJumping && !isDashing && !isWallJumping)
            PlayerState_ = PlayerState.Walking;

        if ((horizontal==0 || rb.velocity==Vector2.zero) && !isJumping)
            PlayerState_ = PlayerState.Idle;

        if (rb.velocity.y<0)
            PlayerState_ = PlayerState.Falling;

        if (isDashing)
            PlayerState_ = PlayerState.Dashing;


        anim.SetInteger("State", (int)PlayerState_);
        anim.SetBool("isRight", IsRight);
    }
    

    public void UnlockAbility(ItemType type)
    {
        Debug.Log("done did do");
        if (type == ItemType.DashAbility)
            data.isDashUnlocked = true;
        else if (type == ItemType.DoubleJumpAbility)
            data.jumpAmount = 2;
        else if (type == ItemType.WallJumpAbility)
            data.isWallJumpUnlocked = true;
        else if (type == ItemType.HeartContainer)
        {
            PlayerAttackBehaviour player = GetComponent<PlayerAttackBehaviour>();
            if (player.CurrentHealth<data.playerHealthNum)
            {
                player.SetHealth(1);
            }
        }
    }
    public void SetStateToAttack()
    {
        PlayerState_ = PlayerState.Attacking;
    }
}
