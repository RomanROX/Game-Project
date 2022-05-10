using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ParameterType
{
    doubleJump,
    dash
}

public class BorderWall : MonoBehaviour
{
    [SerializeField] ParameterType type;
    private void Start()
    {
        switch (type)
        {
            case ParameterType.doubleJump:
                if (GameManager.Instance.PlayerData.jumpAmount==2)
                    gameObject.SetActive(false);
                break;
            case ParameterType.dash:
                if (GameManager.Instance.PlayerData.isDashUnlocked == true)
                    gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }
}
