using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSaveData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.GetString("ProgressLoadedBefore").Equals("Yes"))
        {
            ResetData();
        }
    }

    public void ResetData()
    {
        PlayerPrefs.SetString("ProgressLoadedBefore", "Yes");
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.SetInt("SelfishEndAchieved", 0);
        PlayerPrefs.SetInt("NeutralEndAchieved", 0);
        PlayerPrefs.SetInt("SelflessEndAchieved", 0);
        PlayerPrefs.SetInt("AbruptEndAchieved", 0);
    }

    public void SetScore(int newScore)
    {
        PlayerPrefs.SetInt("Score", 0);
    }
    public void ClearScore()
    {
        SetScore(0);
    }
    public void ModifyScore(int scoreModifier) //Call this and add a +1 or -1.
    {
        PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + scoreModifier);
    }
    public int GetScore()
    {
        return PlayerPrefs.GetInt("Score");
    }

    public void SaveSelfishEndingAchieved(bool endingAchieved)
    {
        if (endingAchieved == true)
        {
            PlayerPrefs.SetInt("SelfishEndAchieved", 1);
        } else
        {
            PlayerPrefs.SetInt("SelfishEndAchieved", 0);
        }
    }
    public void SaveNeutralEndingAchieved(bool endingAchieved)
    {
        if (endingAchieved == true)
        {
            PlayerPrefs.SetInt("NeutralEndAchieved", 1);
        }
        else
        {
            PlayerPrefs.SetInt("NeutralEndAchieved", 0);
        }
    }
    public void SaveSelflessEndingAchieved(bool endingAchieved)
    {
        if (endingAchieved == true)
        {
            PlayerPrefs.SetInt("SelflessEndAchieved", 1);
        }
        else
        {
            PlayerPrefs.SetInt("SelflessEndAchieved", 0);
        }
    }
    public void SaveAbruptEndingAchieved(bool endingAchieved)
    {
        if (endingAchieved == true)
        {
            PlayerPrefs.SetInt("AbruptEndAchieved", 1);
        }
        else
        {
            PlayerPrefs.SetInt("AbruptEndAchieved", 0);
        }
    }

    public bool GetSelfishEndAchieved()
    {
        if(PlayerPrefs.GetInt("SelfishEndAchieved") == 0)
        {
            return false;
        } else
        {
            return true;
        }
    }
    public bool GetNeutralEndAchieved()
    {
        if (PlayerPrefs.GetInt("NeutralEndAchieved") == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public bool GetSelflessEndAchieved()
    {
        if (PlayerPrefs.GetInt("SelflessEndAchieved") == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public bool GetAbruptEndAchieved()
    {
        if (PlayerPrefs.GetInt("AbruptEndAchieved") == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
