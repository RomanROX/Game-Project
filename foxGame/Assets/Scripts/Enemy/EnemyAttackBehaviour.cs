using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBehaviour : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] float maxHealth;
    [SerializeField] float timeBeforeDeath;
    float currentHealth;

    [Header("Attack")]
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange;
    [SerializeField] float attackDamage;
    [SerializeField] float attackRate;

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
            Attack();
            attackTimer = attackRate;
        }
    }

    void Attack()
    {
        //players[0].GetComponent<EnemyAttackBehaviour>().TakeDamage(attackDamage);

        foreach (Collider2D player in players)
        {
            player.GetComponent<PlayerAttackBehaviour>().TakeDamage(attackDamage);
        }
    }

    public void TakeDamage(float dmg)
    {
        currentHealth-=dmg;

        if (currentHealth <= 0) StartCoroutine(Die());
    }
    IEnumerator Die()
    {
        yield return new WaitForSeconds(timeBeforeDeath);
        Destroy(gameObject);
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
