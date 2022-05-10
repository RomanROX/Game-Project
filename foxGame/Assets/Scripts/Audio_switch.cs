using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Audio_switch : MonoBehaviour
{
    public string SceneName;

    public GameObject Audio;
    public GameObject BossAudio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == SceneName)
        {
            Audio.gameObject.SetActive(false);
            BossAudio.gameObject.SetActive(true);

        }
        else
        {
            Audio.gameObject.SetActive(true);
            BossAudio.gameObject.SetActive(false);
        }
    }
}
