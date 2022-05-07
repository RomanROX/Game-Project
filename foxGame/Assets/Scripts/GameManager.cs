using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public PlayerData PlayerData { get; private set; }

    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }else { DestroyImmediate(gameObject); }
        PlayerData = GetComponentInChildren<PlayerData>();
    }
}
