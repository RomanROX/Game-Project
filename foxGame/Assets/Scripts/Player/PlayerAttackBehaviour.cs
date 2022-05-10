using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAttackBehaviour : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] Transform leftAttackPoint;
    [SerializeField] Transform rightAttackPoint;
    [SerializeField] float attackRange;
    [SerializeField] float attackDamage;
    [SerializeField] float attackRate;

    LayerMask enemies_;

    [Header("Health")]
    [SerializeField] float timeBeforeDeath;

    [SerializeField] bool isRight;

    float currentHealth;
    public float CurrentHealth => currentHealth;
    float attackTimer;
    List<Collider2D> enemies = new List<Collider2D>();

    private void Start()
    {
        currentHealth = GameManager.Instance.PlayerData.playerHealthNum;
        enemies_ = GameManager.Instance.LayerHolder.Enemy_;
    }
    private void Update()
    {
        attackTimer -= Time.deltaTime;

        CheckForEnemies();

        if (Input.GetKeyDown(KeyCode.E) && attackTimer <= 0)
        {
            GetComponent<PlayerMovement>().SetStateToAttack();

            Debug.Log("should Attack");
            Attack();
            attackTimer = attackRate;
        }


        //Physics2D.OverlapCircleAll()
    }

    void CheckForEnemies()
    {
        enemies = new List<Collider2D>();

        enemies.AddRange(Physics2D.OverlapCircleAll(leftAttackPoint.position, attackRange, GameManager.Instance.LayerHolder.Enemy_));
        enemies.AddRange(Physics2D.OverlapCircleAll(rightAttackPoint.position, attackRange, GameManager.Instance.LayerHolder.Enemy_));
    }

    void Attack()
    {
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.CompareTag("boss"))
            {
                Debug.Log("Attack the boss");
                enemy.GetComponent<BossBase>().TakeDamage(attackDamage);
            }

            else if (enemy.CompareTag("Wolf"))
                enemy.GetComponent<WolfBehaviour>().TakeDamage(attackDamage);
            else if (enemy.CompareTag("Owl"))
                enemy.GetComponent<OwlBehaviour>().TakeDamage(attackDamage);
        }
    }
    public void SetHealth(float num)
    {
        currentHealth += num;
        if (currentHealth>GameManager.Instance.PlayerData.playerHealthNum)
            currentHealth = GameManager.Instance.PlayerData.playerHealthNum;

        GameManager.Instance.UpdateHeartState(currentHealth);

        Debug.Log("Player health: " + currentHealth);
        if (currentHealth <= 0) StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        Debug.Log("Player died");
        yield return new WaitForSeconds(timeBeforeDeath);
        SceneManager.LoadScene("DeathScene");
        //Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (leftAttackPoint == null || rightAttackPoint==null) return;
        Gizmos.DrawWireSphere(leftAttackPoint.position, attackRange);
        Gizmos.DrawWireSphere(rightAttackPoint.position, attackRange);
    }
}
