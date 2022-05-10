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

    bool isPickup;
    Collider2D player;
    
    //LayerHolder layerHolder;


    private void Update()
    {
        checkForPlayer();
        if (isPickup)
        {
            player.GetComponent<PlayerMovement>().UnlockAbility(itemType);
        }
    }
    void checkForPlayer()
    {
        if (Physics2D.OverlapBox(transform.position, colisionRadious, 0f, GameManager.Instance.LayerHolder.Player))
        {
            player = Physics2D.OverlapBox(transform.position, colisionRadious, 0f, GameManager.Instance.LayerHolder.Player);
            isPickup = true;
            Destroy(this.gameObject);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, colisionRadious);
    }

}
