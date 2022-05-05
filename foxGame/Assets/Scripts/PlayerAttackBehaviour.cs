using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackBehaviour : MonoBehaviour
{
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] float attackDamage;
    [SerializeField] float attackRate;

    float attackTimer;
    List<Collider2D> enemies = new List<Collider2D>();

    private void Update()
    {
        attackTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E) && attackTimer <= 0)
        {
           // Debug.Log("attack");
            Attack();
            attackTimer = attackRate;
        }

    }

    void Attack()
    {
         //   Debug.Log("ya gonna work bro?");
        enemies = new List<Collider2D>(Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer));
        
        foreach (Collider2D enemy in enemies)
        {
            //Debug.Log("Hit "+enemy.name);
            enemy.GetComponent<EnemyBase>().TakeDamage(attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
