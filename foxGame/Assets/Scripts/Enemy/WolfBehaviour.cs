using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfBehaviour : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] float maxHealth;
    [SerializeField] float timeBeforeDeath;
    float currentHealth;

    [Header("Attack")]
    [SerializeField] Vector2 attackRange;
    [SerializeField] float attackDamage;
    [SerializeField] float dashTime;
    //[SerializeField] float attackRate;
    [SerializeField] GameObject drop;
    [SerializeField] Transform attackPos;

    [Header("Movement")]
    [SerializeField] float sightRange;
    [SerializeField] float chargeTime;
    [SerializeField] float speed;
    bool isDashing;

    LayerMask playerMask;
    //float chargeTimer;
    float dashTimer;
    List<Collider2D> players = new List<Collider2D>();
    GameObject playerObj;
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;


    private void Start()
    {
        currentHealth = maxHealth;
        playerObj = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        playerMask = GameManager.Instance.LayerHolder.Player;
    }
    private void Update()
    {
        //chargeTimer -= Time.deltaTime;
        dashTimer -= Time.deltaTime;

        if (isDashing)
            CheckForPlayerInAttackRange();

        if (CanSeePlayer() && /*chargeTimer <= 0&& */dashTimer<=0 && !isDashing)
        {
            Debug.Log("should start dash");
            StartCoroutine(Charge());
            //chargeTimer = chargeTime;
        }
        else if (dashTimer <= 0 && isDashing)
        {
            StopDash();
        }
    }


    float FindDashDir()
    {
        Vector2 distance = playerObj.transform.position-transform.position;
        float dir = distance.normalized.x;
        if (dir ==1)
            sr.flipX = false;
        else if (dir ==-1)
            sr.flipX = true;
        return dir;
    }

    IEnumerator Charge()
    {
        Debug.Log("charging");
        anim.SetTrigger("isCharging");
        anim.ResetTrigger("Reset");
        yield return new WaitForSeconds(chargeTime);
        anim.ResetTrigger("isCharging");
        StartDash();
    }

    void StartDash()
    {
        Debug.Log("started dash");
        isDashing = true;
        dashTimer = dashTime;
        rb.velocity = Vector2.right*FindDashDir() * speed;
        anim.SetTrigger("isDashing");
    }
    void StopDash()
    {
        Debug.Log("Ended dash");
        rb.velocity = Vector2.zero;
        isDashing=false;
        anim.ResetTrigger("isDashing");
        anim.SetTrigger("Reset");
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0) StartCoroutine(Die());
    }
    IEnumerator Die()
    {
        Debug.Log("die?");
        yield return new WaitForSeconds(timeBeforeDeath);
        DropItem();
        Destroy(gameObject);
    }

    void DropItem()
    {
        if (drop != null)
        {
            Instantiate(drop, new Vector3(transform.position.x, transform.position.y + 0.2f), Quaternion.identity);
        }
    }

    void CheckForPlayerInAttackRange()
    {
        players = new List<Collider2D>();

        players.AddRange(Physics2D.OverlapBoxAll(attackPos.position, attackRange, 0f, playerMask));

        if (players.Count > 0)
        {
            foreach (Collider2D player in players)
            {
                player.GetComponent<PlayerAttackBehaviour>().SetHealth(-attackDamage);
            }
        }
    }

    bool CanSeePlayer()
    {
        return Physics2D.OverlapCircle(transform.position, sightRange, playerMask);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(attackPos.position, attackRange);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, sightRange);

    }
}
