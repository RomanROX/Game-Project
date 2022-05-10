using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public PlayerData PlayerData { get; private set; }
    public LayerHolder LayerHolder{ get; private set; }

    public GameObject HealthUI;
    public GameObject PauseUI;

    public List<Sprite> UIHeartStates;
    public List<Image> UIHearts;

    bool isPaused = false;

    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }else { Debug.Log("Destroied gm"); DestroyImmediate(gameObject); }

        PlayerData = Instance.GetComponentInChildren<PlayerData>();
        LayerHolder = Instance.GetComponentInChildren<LayerHolder>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            PauseUI.SetActive(isPaused);
            Time.timeScale = isPaused? 0.0f : 1.0f;
        }
    }

    public void AddMaxHealth(int num)
    {
        UIHearts[num-1].gameObject.SetActive(true);
        for (int i = 0; i < num; i++)
        {
            UIHearts[i].sprite = UIHeartStates[(int)UIHeartState.whole];
        }
    }
    public void UpdateHeartState(float health)
    {
        int halfpoint = (int)(health / 2 + 0.5f);
        //Debug.Log(Mathf.Abs(health / 2));
        if (health/2 != Mathf.Round(health / 2))
        {
            Debug.Log("health is half");
            UIHearts[halfpoint-1].sprite = UIHeartStates[(int)UIHeartState.half];
        }

        int empty = halfpoint + 1;

        if (empty <= PlayerData.playerHealthNum/2)
        {
            Debug.Log("health is empty");
            UIHearts[empty-1].sprite = UIHeartStates[(int)UIHeartState.empty];
        }
        if (halfpoint>0)
        {
            Debug.Log("health is full");
            for (int i = halfpoint-1; i > 0; i--)
            {
                UIHearts[i-1].sprite = UIHeartStates[(int)UIHeartState.whole];
            }
        }
        Debug.Log("halfpoint: " + halfpoint + "\nempty: " + empty);
    }
    public void ResumePauseMenu()
    {
        isPaused=false;
        PauseUI.SetActive(false);
        Time.timeScale = 1;
    }
}
