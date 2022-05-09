using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase : MonoBehaviour
{
    [SerializeField] float maxHealth;
    [SerializeField] float speed;
    [SerializeField] float dashSpeed;
    [SerializeField] float fallSpeed;
    [SerializeField] float attackDamage;
    [SerializeField] float gravityScale;

    [SerializeField] Vector2 defaultSpot;
    [SerializeField] Vector2 restingSpotLeft;
    [SerializeField] Vector2 restingSpotRight;
    [SerializeField] Vector2 fallAttackSpot;

    [SerializeField] GameObject rockSpawner;
    [SerializeField] GameObject player;

    [SerializeField] Transform collidPos;
    [SerializeField] Vector2 collidRange;

    float currentHealth;
    float attackTimer;
    List<Collider2D> playerList;

    public Vector2 FallAttackSpot => fallAttackSpot;
    public Vector2 DefaultSpot => defaultSpot;
    public float Speed => speed;
    public float FallSpeed => fallSpeed;
    public float DashSpeed => dashSpeed;
    public float GravityScale=> gravityScale;
    public GameObject PlayerObj => player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = maxHealth;
    }

    private void Update()
    {
        attackTimer -= Time.deltaTime;

        CheckForPlayer();

        if (playerList.Count > 0 && attackTimer <= 0)
        {
            StartCoroutine(Attack());
            attackTimer = 0.5f;
        }

    }

    public Vector2 GetRandomRestingSpot()
    {
        float result = Random.Range(5, -5);
        return (Mathf.Sign(result)==1)? restingSpotRight : restingSpotLeft;
    }

    public void StartRockAttacking()
    {
        rockSpawner.SetActive(true);
    }
    public void StopRockAttacking()
    {
        rockSpawner.SetActive(false);
    }

    void CheckForPlayer()
    {

        playerList = new List<Collider2D>(); 
        playerList.AddRange(Physics2D.OverlapBoxAll(collidPos.position, collidRange, 0f, GameManager.Instance.LayerHolder.Player));
        
        Debug.Log(playerList.Count);
    }
    IEnumerator Attack()
    {
        foreach (Collider2D player in playerList)
        {
            player.GetComponent<PlayerAttackBehaviour>().SetHealth(-attackDamage);
        }
        yield return null;
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        Debug.Log("enemy health " + currentHealth);

        if (currentHealth<=0)
            Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(collidPos.position, collidRange);
    }
}
