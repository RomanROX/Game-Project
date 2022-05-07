using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_buttons : MonoBehaviour
{
    public string SceneName = "";  
   

    //Play button
    public void Play()
    {
        SceneManager.LoadScene(SceneName);
    }
    //Quit button
    public void Quit()
    {
        Application.Quit();
    }

}
