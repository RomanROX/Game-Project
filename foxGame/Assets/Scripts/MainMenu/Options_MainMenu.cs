using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options_MainMenu : MonoBehaviour
{
  
    public void SetMasterVolume(float Master_volume)
    {
        Debug.Log("Master " + Master_volume);
    }

    public void SetFXVolume(float FX_volume)
    {
        Debug.Log("FX " +FX_volume);
    }

    public void SetMusicVolume(float Music_volume)
    {
        Debug.Log("Music " + Music_volume);
    }

}
