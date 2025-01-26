using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsSaveData : MonoBehaviour
{
    void Start()
    {
        if (!PlayerPrefs.GetString("LoadedBefore").Equals("Yes"))
        {
            ResetData();
        }
    }

    public void ResetData()
    {
        PlayerPrefs.SetString("LoadedBefore", "Yes");
        PlayerPrefs.SetFloat("SFXVolume", 5);
        PlayerPrefs.SetFloat("BGMVolume", 5);
    }

    public void SaveSFXVolume(float newVolume)
    {
        PlayerPrefs.SetFloat("SFXVolume", newVolume);
    }

    public void SaveBGMVolume(float newVolume)
    {
        PlayerPrefs.SetFloat("BGMVolume", newVolume);
    }

    public float LoadSFXVolume()
    {
        return PlayerPrefs.GetFloat("SFXVolume");
    }

    public float LoadBGMVolume()
    {
        return PlayerPrefs.GetFloat("BGMVolume");
    }
}
