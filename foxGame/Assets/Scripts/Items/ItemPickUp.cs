using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    DashAbility,
    DoubleJumpAbility,
    WallJumpAbility,
    HeartContainer
}

public class ItemPickUp : MonoBehaviour
{
    [SerializeField] Vector2 colisionRadious;
    [SerializeField] ItemType itemType;
    
    //LayerHolder layerHolder;


    private void Update()
    {
        checkForPlayer();
    }
    void checkForPlayer()
    {
        if (Physics2D.OverlapBox(transform.position, colisionRadious, 0f, GameManager.Instance.LayerHolder.Player))
        {
            Collider2D obj = Physics2D.OverlapBox(transform.position, colisionRadious, 0f, GameManager.Instance.LayerHolder.Player);
            obj.GetComponent<PlayerMovement>().UnlockAbility(itemType);
            Destroy(gameObject);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, colisionRadious);
    }

}
