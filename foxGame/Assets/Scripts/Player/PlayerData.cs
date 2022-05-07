using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int playerHealthNum;
    
    public float coyoteTime = 0.5f;
    public float jumpBufferTime = 0.5f;
    
    public float runAccel = 9.3f;
    public float runDeccel = 15f;
    public float airAccel = 0.65f;
    
    public float wallJumpTime = 0.35f;
    public float dashAttackTime = 0.15f;
    public float dashEndTime = 0.1f;
    public float dashBufferTime = 0.05f;


    public float speed = 15;
    public float jumpSpeed = 20;
    public float gravityScale = 10;
    public float fallingGravityScale = 7;
    public float dashSpeed = 20;
    public Vector2 wallJumpForce = new Vector2(9, 12);
    
    public int dashAmount = 1;
    public int jumpAmount = 1;

    public bool isDashUnlocked;
    public bool isWallJumpUnlocked;
}
