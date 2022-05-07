using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBehaviour : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] float maxHealth;
    [SerializeField] float timeBeforeDeath;
    [SerializeField] float timeBeforeAttack;
    float currentHealth;

    [Header("Attack")]
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange;
    [SerializeField] float attackDamage;
    [SerializeField] float attackRate;
    [SerializeField] GameObject drop;

    float attackTimer;
    List<Collider2D> players = new List<Collider2D>();

    private void Start()
    {
        currentHealth = maxHealth;
    }
    private void Update()
    {
        attackTimer -= Time.deltaTime;

        CheckForPlayer();

        if (players.Count > 0 && attackTimer <= 0)
        {
            StartCoroutine(Attack());
            attackTimer = attackRate;
        }
    }

   

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(timeBeforeAttack);
        foreach (Collider2D player in players)
        {
            player.GetComponent<PlayerAttackBehaviour>().SetHealth(-attackDamage);
        }
    }

    public void TakeDamage(float dmg)
    {
        currentHealth-=dmg;

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
            Instantiate(drop, new Vector3(transform.position.x, transform.position.y+0.2f), Quaternion.identity);
        }
    }

    void CheckForPlayer()
    {
        players = new List<Collider2D>(Physics2D.OverlapCircleAll(attackPoint.position, attackRange, LayerHolder.Instance.Player));
    }
    public void ChangeAttackPos()
    {
        attackPoint.Translate(new Vector3(-attackPoint.position.x, attackPoint.position.y));
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
