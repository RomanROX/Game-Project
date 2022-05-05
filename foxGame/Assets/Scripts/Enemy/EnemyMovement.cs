using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float playerDetectionRange;
    [SerializeField] float speed;
    [SerializeField] LayerMask isPlayer;
    [SerializeField] float runAccel;
    [SerializeField] float runDeccel;
    [SerializeField] float stopDistance;

    bool isDetectingPlayer;
    Rigidbody2D rb;
    List<Collider2D> player;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        PlayerProximityCheck();
    }
    private void FixedUpdate()
    {
        if (isDetectingPlayer && player != null)
        {
            float diff = transform.position.x - player[0].transform.position.x;

            
            float dir = Mathf.Sign(diff);
            if (Mathf.Abs(diff) < stopDistance) dir =0;
            Move(-dir);
        }
    }

    void PlayerProximityCheck()
    {
        player = new List<Collider2D>(Physics2D.OverlapCircleAll(transform.position, playerDetectionRange, isPlayer));
        isDetectingPlayer = Physics2D.OverlapCircle(transform.position, playerDetectionRange, isPlayer);
    }
    void Move(float moveDir)
    {
        float speedX = speed * moveDir;
        float force = speedX - rb.velocity.x;

        float accelRate;
        
        accelRate = (Mathf.Abs(speedX) > 0.01f) ? runAccel : runDeccel;
       
        float final = force * accelRate;
        rb.AddForce(Vector2.right * final);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRange);
    }
}
