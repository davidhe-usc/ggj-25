using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class EndingManager : MonoBehaviour
{
    DialogueRunner dr;

    // Start is called before the first frame update
    void Start()
    {
        dr = FindObjectOfType<DialogueRunner>();

        if (PlayerPrefs.GetInt("Score") >= 11)
        {
            dr.StartDialogue("Ending2-Selfless");
        }
        else if (PlayerPrefs.GetInt("Score") <= -2)
        {
            dr.StartDialogue("Ending1-Selfish");
        }
        else
        {
            dr.StartDialogue("Ending3-Neutral");
        }
    }

   
    // Update is called once per frame
    void Update()
    {
        
    }
}
