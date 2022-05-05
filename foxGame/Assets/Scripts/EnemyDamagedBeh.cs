using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] float maxHealth;
    float currentHealth;


    private void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(float dmg)
    {
        currentHealth-=dmg;
        //Debug.Log($"enemy health {currentHealth}");

        if (currentHealth <= 0) StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        //Debug.Log("enemy dies");
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }

    //void Die()
    //{
    //    Debug.Log("enemy dies");
    //}
}
