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
    public float speed;
    //public float jump;
    public float horizontal;

    [SerializeField]
    private Transform feetPos;
    [SerializeField]
    private Transform leftWallPos;
    [SerializeField]
    private Transform rightWallPos;
    //private bool CanJump = false;

    private float lastPressedJumpTime;
    private float lastOnGroundTime;
    
    private float lastOnWallRightTime;
    private float lastOnWallLeftTime;
    private float lastOnWallTime;

    private float wallJumpStartTime;
    private float wallJumpTime=0.35f;
    private float lastWallJumpDir;

    public Vector2 groundCheckRadious;
    public Vector2 wallCheckRadious;
    public LayerMask ground;

    private bool jumpdet = true;
    public bool IsRight = true;
    public bool isJumping;
    private bool isWallJumping;

    private PlayerState PlayerState_;

    public float jumpAmount = 35;
    public float gravityScale = 10;
    public float fallingGravityScale = 40;
    public Vector2 wallJumpForce=new Vector2(9,12);

    public Rigidbody2D rb;
    public Animator childAnim;


    public float coyoteTime = 0.5f;
    public float jumpBufferTime = 0.5f;
    public float maxSpeed;

    //public float dragAmount = 0.22f;
    //public float frictionAmount = 0.55f;
    //public float runAccel = 9.3f;
    //public float runDeccel = 15f;
    ////public float airAccel = 0.65f;
    ////public float airDeccel = 0.65f;
    //public float stopPower = 1.23f;
    //public float turnPower = 1.13f;
    //public float accelPower = 1.05f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        childAnim = gameObject.GetComponentInChildren<Animator>();
    }

    public void Update()
    {
        //if (Input.GetKey(KeyCode.U))
        //    Debug.Log("---- " + Time.time);

        horizontal = Input.GetAxisRaw("Horizontal");

        lastPressedJumpTime -= Time.deltaTime;
        lastOnGroundTime -= Time.deltaTime;
        lastOnWallLeftTime -= Time.deltaTime;
        lastOnWallRightTime -= Time.deltaTime;
        lastOnWallTime -= Time.deltaTime;


        if (!isJumping)
        {
            if (Physics2D.OverlapBox(feetPos.position, groundCheckRadious, 0f, ground))
                lastOnGroundTime = coyoteTime;

            if (Physics2D.OverlapBox(leftWallPos.position, wallCheckRadious, 0f, ground)&&!IsRight)
                lastOnWallLeftTime = coyoteTime;

            if (Physics2D.OverlapBox(rightWallPos.position, wallCheckRadious, 0f, ground)&IsRight)
                lastOnWallRightTime = coyoteTime;
            lastOnWallTime = Mathf.Max(lastOnWallRightTime, lastOnWallLeftTime);
        }
        //Debug.Log("vel: " + rb.velocity.y);
        #region JUMP & WALL JUMP
        if (isJumping = true && rb.velocity.y < 0)
        {
            Debug.Log("is jumping = false");
            
            isJumping = false;
        }
        if (CanJump() && lastPressedJumpTime>0)
        {
            isJumping = true;
            Debug.Log("is jumping" + isJumping);
            isWallJumping = false;
            Jump();
        }
        else if(CanWallJump() && lastPressedJumpTime > 0)
        {
            isWallJumping=true;
            isJumping = false;
            wallJumpStartTime = Time.time;
            lastWallJumpDir = (lastOnWallRightTime > 0) ? -1 : 1;

            WallJump();
        }
        #endregion
        if (rb.velocity.y >= 0)
            rb.gravityScale = gravityScale;
        else
            rb.gravityScale = fallingGravityScale;


        if (isWallJumping && Time.time -wallJumpStartTime>wallJumpTime)
            isWallJumping=false;


        InputCallbacks();

        animate();
        
        Debug.Log("Playerstate: "+PlayerState_);


    }

    private void FixedUpdate()
    {
        //Jump();
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
    }
    private bool CanJump()
    {
        Debug.Log("is jumping" + isJumping);
        return lastOnGroundTime>0 && !isJumping;
        
    }
    private bool CanWallJump()
    {
        return lastPressedJumpTime > 0 && lastOnWallTime > 0 && lastOnWallTime <= 0 &&
            (!isWallJumping || (lastOnWallRightTime > 0 && lastWallJumpDir == 1) ||
            (lastOnWallLeftTime > 0 && lastWallJumpDir == -1));
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

        if (Mathf.Sign(rb.velocity.x) != Mathf.Sign(force.x))
            force.x -= rb.velocity.x;
        if (rb.velocity.y<0)
            force.y-= rb.velocity.y;
        Debug.Log(force);

        rb.AddForce(force, ForceMode2D.Impulse);
    }
    private void Move()
    {
        float speedX = speed * horizontal;
        
        speedX = Mathf.Lerp(rb.velocity.x, speedX, 1);

        rb.velocity = new Vector2(speedX, rb.velocity.y);

        //rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }
    private void animate()
    {
        Debug.Log("animation");
        if (rb.velocity.y==0)
        {
            PlayerState_ = PlayerState.Walking;
            Debug.Log("walkChexh");
        }
        if (rb.velocity.x>0) { IsRight = true; }
        else if (rb.velocity.x<0) { IsRight = false; }

        childAnim.SetFloat("PlayerState", (float)PlayerState_);
        childAnim.SetBool("IsRight", IsRight);
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
