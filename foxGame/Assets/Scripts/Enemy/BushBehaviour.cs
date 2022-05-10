using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BushBehaviour : MonoBehaviour
{
    [SerializeField] Vector2 sizeCollisionCheck;
    LayerMask playerMask;
    //[SerializeField] Transform originPos;

    private void Start()
    {
        playerMask = GameManager.Instance.LayerHolder.Player;
    }

    private void Update()
    {
        CheckForPlayer();
    }

    void CheckForPlayer()
    {
        if (Physics2D.OverlapBox(transform.position, sizeCollisionCheck, 0f, playerMask))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, sizeCollisionCheck);
    }
}
