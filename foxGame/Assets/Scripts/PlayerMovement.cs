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
    public float horizontal;

    [SerializeField]
    private Transform feetPos;
    [SerializeField]
    private Transform leftWallPos;
    [SerializeField]
    private Transform rightWallPos;

    private float lastPressedJumpTime;
    private float lastOnGroundTime;

    private float lastOnWallRightTime;
    private float lastOnWallLeftTime;
    private float lastOnWallTime;

    private float wallJumpStartTime;
    private float wallJumpTime = 0.35f;
    private float lastWallJumpDir;

    public Vector2 groundCheckRadious;
    public Vector2 wallCheckRadious;
    public LayerMask ground;

    public bool IsRight = true;
    public bool isJumping;
    private bool isWallJumping;

    private PlayerState PlayerState_;

    public float jumpAmount = 35;
    public float gravityScale = 10;
    public float fallingGravityScale = 40;
    public Vector2 wallJumpForce = new Vector2(9, 12);

    public Rigidbody2D rb;
    public Animator childAnim;


    public float coyoteTime = 0.5f;
    public float jumpBufferTime = 0.5f;

    public float runAccel = 9.3f;
    public float runDeccel = 15f;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        childAnim = gameObject.GetComponentInChildren<Animator>();
    }

    public void Update()
    {
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

            if (Physics2D.OverlapBox(leftWallPos.position, wallCheckRadious, 0f, ground) && !IsRight)
                lastOnWallLeftTime = coyoteTime;

            if (Physics2D.OverlapBox(rightWallPos.position, wallCheckRadious, 0f, ground) & IsRight)
                lastOnWallRightTime = coyoteTime;
            lastOnWallTime = Mathf.Max(lastOnWallRightTime, lastOnWallLeftTime);
        }
        #region JUMP & WALL JUMP
        if (isJumping && rb.velocity.y <= 0)
        {

            isJumping = false;
        }

        if (CanJump() && lastPressedJumpTime > 0)
        {
            isJumping = true;
            isWallJumping = false;
            Jump();
        }
        else if (CanWallJump() && lastPressedJumpTime > 0)
        {
            Debug.Log("wall jump?");
            isWallJumping = true;
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


        if (isWallJumping && Time.time - wallJumpStartTime > wallJumpTime)
            isWallJumping = false;


        InputCallbacks();

        animate();

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
        Debug.Log(force);

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
            accelRate = 0.65f;

        float final = force * accelRate;
        rb.AddForce(Vector2.right * final);
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
