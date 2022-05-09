using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlRockBehaviour : MonoBehaviour
{
    Transform player;
    [SerializeField] float speed;
    [SerializeField] float size;
    Vector3 pos;
    LayerMask playerLayer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(DestroySelf(1.8f));
        pos = player.position;
        playerLayer = GameManager.Instance.LayerHolder.Player;
        //MoveTowards();
    }

    private void Update()
    {
        MoveTowards();

        if (Physics2D.OverlapCircle(transform.position, size, playerLayer))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttackBehaviour>().SetHealth(-1);
            Destroy(gameObject);
        }
    }
    void MoveTowards()
    {
        //Vector2 dir = player.position-transform.position;

        //Vector2 force = new Vector2(dir.normalized.x, (dir.y>0)? dir.y+0.5f:dir.y-0.5f)* speed;
        //GetComponent<Rigidbody2D>().velocity = force;

        Vector3 moveDir = (pos - transform.position).normalized;
        transform.position += moveDir * speed * Time.deltaTime;
        //Vector2 force = Vector2.MoveTowards(transform.position, player.position, )
    }

    IEnumerator DestroySelf(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }


}
