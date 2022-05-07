using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackBehaviour : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange;
    [SerializeField] float attackDamage;
    [SerializeField] float attackRate;

    [Header("Health")]
    [SerializeField] float maxHealth;
    [SerializeField] float timeBeforeDeath;

    [SerializeField] bool isRight;

    float currentHealth;
    float attackTimer;
    List<Collider2D> enemies = new List<Collider2D>();

    private void Start()
    {
        currentHealth = maxHealth;
    }
    private void Update()
    {
        attackTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E) && attackTimer <= 0)
        {
            Attack();
            attackTimer = attackRate;
        }

    }

    void Attack()
    {
        enemies = new List<Collider2D>(Physics2D.OverlapCircleAll(attackPoint.position, attackRange, LayerHolder.Instance.Enemy));
        
        foreach (Collider2D enemy in enemies)
        {
            enemy.GetComponent<EnemyAttackBehaviour>().TakeDamage(attackDamage);
        }
    }
    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0) StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        Debug.Log("Player died");
        yield return new WaitForSeconds(timeBeforeDeath);
        //Destroy(gameObject);
    }
    //public void ChangeAttackPos()
    //{
    //    attackPoint.Translate(new Vector3(-attackPoint.position.x, attackPoint.position.y));
    //}

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
