using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropBehaviour : MonoBehaviour
{
    [SerializeField] Vector2 MaxMovementDir;
    [SerializeField] Vector2 MinMovementDir;
    [SerializeField] float frictionAmount;

    private void Start()
    {
        float x = Random.Range(MaxMovementDir.x, MinMovementDir.x);
        float y = Random.Range(MinMovementDir.y, MaxMovementDir.y);
        Vector2 dir = new Vector2(x, y);
        GetComponent<Rigidbody2D>().AddForce(dir, ForceMode2D.Impulse);

    }
    private void Update()
    {
        Friction();
    }
    void Friction()
    {
        if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y)<0.1f)
        {
            float dir = GetComponent<Rigidbody2D>().velocity.normalized.x;
            GetComponent<Rigidbody2D>().AddForce(Vector2.left * dir * frictionAmount);
        }
    }
}
