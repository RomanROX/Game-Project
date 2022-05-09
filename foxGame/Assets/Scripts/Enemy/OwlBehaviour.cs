using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlBehaviour : MonoBehaviour
{
    [SerializeField] float sightRange;
    [SerializeField] float maxHealth;
    [SerializeField] float timeBetweenAttacks;
    [SerializeField] float timeBeforeDeath;
    [SerializeField] GameObject rock;
    [SerializeField] List<Sprite> rockSprites;
    [SerializeField] GameObject drop;


    List<Collider2D> players = new List<Collider2D>();
    LayerMask playerMask;
    bool isAttacking;
    Animator anim;
    float currentHealth;

    private void Start()
    {
        playerMask = GameManager.Instance.LayerHolder.Player;
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        CheckForPlayerInSight();

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

    void CheckForPlayerInSight()
    {
        players = new List<Collider2D>();

        players.AddRange(Physics2D.OverlapCircleAll(transform.position, sightRange, playerMask));

        if (players.Count > 0 && !isAttacking)
        {
            StartCoroutine(SpawnRock());
            isAttacking = true;
        }
        else if (players.Count == 0 && isAttacking)
        {
            StopAllCoroutines();
        }
    }

    IEnumerator SpawnRock()
    {

        anim.SetTrigger("Throw");
        GameObject obj = rock;
        obj.GetComponent<SpriteRenderer>().sprite = GetRandomRockSprite();
        Instantiate(obj, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(timeBetweenAttacks);
        anim.ResetTrigger("Throw");
        StartCoroutine(SpawnRock());
    }

    Sprite GetRandomRockSprite()
    {
        int num = Random.Range(0, rockSprites.Count);
        return rockSprites[num];
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, sightRange);

    }
}
