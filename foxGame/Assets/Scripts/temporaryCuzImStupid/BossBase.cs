using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase : MonoBehaviour
{
    [SerializeField] float maxHealth;
    [SerializeField] float speed;
    [SerializeField] float dashSpeed;
    [SerializeField] float fallSpeed;
    [SerializeField] float attackDamageDash;
    [SerializeField] float attackDamageFall;

    [SerializeField] Vector2 restingSpotLeft;
    [SerializeField] Vector2 restingSpotRight;

    [SerializeField] GameObject rockSpawner;
    [SerializeField] GameObject player;

    float currentHealth;

    public float Speed => speed;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public Vector2 GetRandomRestingSpot()
    {
        float result = Random.Range(1, -1);
        Debug.Log(result);
        return (Mathf.Sign(result)==1)? restingSpotRight : restingSpotLeft;
    }
}
