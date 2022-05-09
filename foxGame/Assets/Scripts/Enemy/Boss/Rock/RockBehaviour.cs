using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBehaviour : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DestroySelf(1.8f));
    }

    IEnumerator DestroySelf(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerAttackBehaviour>().SetHealth(-1);
            Destroy(gameObject);
        }
    }
}
