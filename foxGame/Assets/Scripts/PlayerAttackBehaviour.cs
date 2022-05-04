using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackBehaviour : MonoBehaviour
{
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange;
    [SerializeField] LayerMask enemyLayer;

    private void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            Attack();
        }
    }

    void Attack()
    {
        List<Collider2D> enemies = new List<Collider2D>(Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer));
        
        foreach (Collider2D enemy in enemies)
        {
            Debug.Log("Hit "+enemy.name);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
