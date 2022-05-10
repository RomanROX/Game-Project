using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButtonBehaviour : MonoBehaviour
{
    public void b_Resume()
    {
        GameManager.Instance.ResumePauseMenu();
    }
    public void b_QuitToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
