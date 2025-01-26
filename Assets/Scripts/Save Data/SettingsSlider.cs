using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSlider : MonoBehaviour
{
    enum WhichSetting { SFXVolume, BGMVolume};

    [SerializeField]
    SettingsSaveData settingsSaveData;
    [SerializeField]
    Slider thisSlider;
    [SerializeField]
    WhichSetting whichSettingToChange;

    bool listeningForInput = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if (settingsSaveData == null)
        {
            settingsSaveData = FindObjectOfType<SettingsSaveData>();
        }
        switch(whichSettingToChange)
        {
            case WhichSetting.SFXVolume:
                thisSlider.value = settingsSaveData.LoadSFXVolume();
                break;
            case WhichSetting.BGMVolume:
                thisSlider.value = settingsSaveData.LoadBGMVolume();
                break;
            default:
                Debug.Log("No setting match found");
                break;
        }
        listeningForInput = true;
    }

    public void UpdateSetting()
    {
        if (listeningForInput == true)
        {
            switch (whichSettingToChange)
            {
                case WhichSetting.SFXVolume:
                    settingsSaveData.SaveSFXVolume(thisSlider.value);
                    break;
                case WhichSetting.BGMVolume:
                    settingsSaveData.SaveBGMVolume(thisSlider.value); 
                    break;
                default:
                    Debug.Log("No setting match found");
                    break;
            }
        }
    }
}
