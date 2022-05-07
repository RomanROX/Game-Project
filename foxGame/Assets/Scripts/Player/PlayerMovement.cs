using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum PlayerState
{
    Walking,
    Jumping,
    Falling,
    InWall,
    Dashing,
    Attacking
}


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float horizontal;
    
    [SerializeField]
    private Transform feetPos;
    [SerializeField]
    private Transform leftWallPos;
    [SerializeField]
    private Transform rightWallPos;

    float lastPressedJumpTime;
    float lastOnGroundTime;
    
    float lastOnWallRightTime;
    float lastOnWallLeftTime;
    float lastOnWallTime;
    
    float wallJumpStartTime;
    float wallJumpTime = 0.35f;
    float lastWallJumpDir;

    [SerializeField] Vector2 groundCheckRadious;
    [SerializeField] Vector2 wallCheckRadious;
   
    [SerializeField] bool IsRight = true;
    [SerializeField] bool isJumping;
    bool isWallJumping;

    PlayerState PlayerState_;

    [SerializeField] float jumpAmount = 35;
    [SerializeField] float gravityScale = 10;
    [SerializeField] float fallingGravityScale = 40;
    [SerializeField] Vector2 wallJumpForce = new Vector2(9, 12);

    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator childAnim;


    [SerializeField] float coyoteTime = 0.5f;
    [SerializeField] float jumpBufferTime = 0.5f;
    
    [SerializeField] float runAccel = 9.3f;
    [SerializeField] float runDeccel = 15f;
    [SerializeField] float airAccel = 0.65f;

    float lastDir;
    float currentDir;

    bool isDashing;
    int dashesLeft;
    Vector2 lastDashDir;
    float dashStartTime;
    float lastPressedDashTime;
    bool dashAttacking;
    [SerializeField] float dashAttackTime;
    [SerializeField] float dashEndTime;
    [SerializeField] float dashBufferTime;
    [SerializeField] int dashAmount;
    [SerializeField] float dashSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        childAnim = gameObject.GetComponentInChildren<Animator>();
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
                lastOnGroundTime = coyoteTime;

            if (Physics2D.OverlapBox(leftWallPos.position, wallCheckRadious, 0f, LayerHolder.Instance.Ground) && !IsRight)
                lastOnWallLeftTime = coyoteTime;

            if (Physics2D.OverlapBox(rightWallPos.position, wallCheckRadious, 0f, LayerHolder.Instance.Ground) & IsRight)
                lastOnWallRightTime = coyoteTime;
            lastOnWallTime = Mathf.Max(lastOnWallRightTime, lastOnWallLeftTime);
        }
        #region JUMP & WALL JUMP
        if (isJumping && rb.velocity.y <= 0)
            isJumping = false;

        if (isWallJumping && Time.time - wallJumpStartTime > wallJumpTime)
            isWallJumping = false;

        if (!isDashing)
        {
        if (CanJump() && lastPressedJumpTime > 0)
        {
            isJumping = true;
            isWallJumping = false;
            Jump();
        }
        else if (CanWallJump() && lastPressedJumpTime > 0)
        {
            //Debug.Log("wall jump?");
            isWallJumping = true;
            isJumping = false;
            wallJumpStartTime = Time.time;
            lastWallJumpDir = (lastOnWallRightTime > 0) ? -1 : 1;

            WallJump();
        }
        }
        #endregion

        if (!isDashing)
        {
            if (rb.velocity.y >= 0)
                rb.gravityScale = gravityScale;
            else
                rb.gravityScale = fallingGravityScale;
        }

        if (DashAttackOver())
        {
            Debug.Log("dash over");
            if (dashAttacking)
            {
                dashAttacking = false;
                StopDash(lastDashDir);
            }
            else if (Time.time - dashStartTime > dashAttackTime + dashEndTime)
                isDashing = false;
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

        CheckDir();

        //Debug.Log("Playerstate: "+PlayerState_);


    }

    private void FixedUpdate()
    {
        Move();
        //if (rb.velocity.y <= 0)
        //{
        //    //rb.velocity = new Vector2(rb.velocity.x, -fallingGravityScale);
        //    rb.AddForce(new Vector2(0, -fallingGravityScale), ForceMode2D.Force);
        //    PlayerState_ = PlayerState.Falling;
        //}
    }








    private void InputCallbacks()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            lastPressedJumpTime = jumpBufferTime;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("pressed dash");
            lastPressedDashTime = dashBufferTime;
        }
    }


    private bool CanJump()
    {
        return lastOnGroundTime > 0 && !isJumping;
    }
    private bool CanWallJump()
    {
        bool temp = lastPressedJumpTime > 0 && lastOnWallTime > 0 && lastOnGroundTime <= 0 &&
            (!isWallJumping || (lastOnWallRightTime > 0 && lastWallJumpDir == 1) ||
            (lastOnWallLeftTime > 0 && lastWallJumpDir == -1));
        return temp;
    }
    bool CanDash()
    {
        if (dashesLeft < dashAmount && lastOnGroundTime > 0)
            dashesLeft = dashAmount;
        Debug.Log("(return) can dash? " + (dashesLeft > 0));
        return dashesLeft > 0;
    }
    bool DashAttackOver()
    {
        bool temp = isDashing && Time.time - dashStartTime > dashAttackTime;
        Debug.Log("(return) dash is over? "+temp);
        return temp;
    }

    private void Jump()
    {
        lastPressedJumpTime = 0;
        lastOnGroundTime = 0;

        rb.AddForce(Vector2.up * jumpAmount, ForceMode2D.Impulse);
    }
    private void WallJump()
    {
        lastPressedJumpTime = 0;
        lastOnGroundTime = 0;
        lastOnWallLeftTime = 0;
        lastOnWallRightTime = 0;

        Vector2 force = wallJumpForce;
        force.x *= lastWallJumpDir;

        if (rb.velocity.y < 0)
            force.y -= rb.velocity.y;
        //Debug.Log(force);

        rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void Move()
    {
        float speedX = speed * horizontal;
        float force = speedX - rb.velocity.x;

        float accelRate;
        if (lastOnGroundTime > 0)
            accelRate = (Mathf.Abs(speedX) > 0.01f) ? runAccel : runDeccel;
        else
            accelRate = airAccel;

        float final = force * accelRate;
        rb.AddForce(Vector2.right * final);
    }

    void StartDash(Vector2 dir)
    {
        Debug.Log("should start dash");
        lastOnGroundTime = 0;
        lastPressedDashTime = 0;

        rb.gravityScale = 0;
        rb.velocity = dir.normalized * dashSpeed;
    }
    void StopDash(Vector2 dir)
    {
        Debug.Log("should stop dash");
        rb.gravityScale = gravityScale;

        if (dir.y>0)
        {
            //if (dir.x==0)
            //    rb.AddForce()
            rb.AddForce(Vector2.down * rb.velocity.y, ForceMode2D.Impulse);
        }
    }

    private void animate()
    {
        //Debug.Log("animation");
        if (rb.velocity.y==0)
        {
            PlayerState_ = PlayerState.Walking;
            //Debug.Log("walkChexh");
        }
        if (rb.velocity.x>0) { IsRight = true; }
        else if (rb.velocity.x<0) { IsRight = false; }


        childAnim.SetFloat("PlayerState", (float)PlayerState_);
        childAnim.SetBool("IsRight", IsRight);
    }
    void CheckDir()
    {
        currentDir = rb.velocity.normalized.x;
        if (currentDir != lastDir)
        {
            //GetComponent<PlayerAttackBehaviour>().ChangeAttackPos();
            lastDir = currentDir;
        }

    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //}
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireCube(feetPos, new Vector3(checkRadious, checkRadious, 1));
    //}
}
