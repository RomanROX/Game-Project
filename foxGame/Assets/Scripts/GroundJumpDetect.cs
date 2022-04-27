using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundJumpDetect : MonoBehaviour
{
    public bool jumpdet = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            jumpdet = true;
        }
    }
}
