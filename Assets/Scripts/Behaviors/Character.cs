using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Yarn.Unity;

public class Character : MonoBehaviour
{
    private DialogueRunner dr;
    private SpriteRenderer sr;

    public Sprite[] emotionSprites; //List of sprites for the character for a given scene
    public LineView textBox; //The personal text box for this character

    // Start is called before the first frame update
    void Start()
    {
        dr = FindObjectOfType<DialogueRunner>(); //We only got one per scene
        sr = GetComponent<SpriteRenderer>(); //Get this character's renderer
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [YarnCommand("Emote")]
    public void SetEmote(string emote) //Just sets the emote. Can be called directly when we don't want to change the speaker
    {
        //Set emotion sprite. Currently our options are:
        //Neutral: 0
        //Sad: 1
        //Happy: 2

        switch (emote)
        {
            case ("Neutral"):
                sr.sprite = emotionSprites[0];
                break;
            case ("Sad"):
                sr.sprite = emotionSprites[1];
                break;
            case ("Happy"):
                sr.sprite = emotionSprites[2];
                break;
            default:
                Debug.LogWarning("No valid emotion, default to neutral");
                sr.sprite = emotionSprites[0];
                break;
        }
    }

    [YarnCommand("PrepareLine")]
    public void PrepareLine(string emote) //Prepares the dialogue runner to display lines from this character. Call again to change emotion
    {
        dr.dialogueViews[0] = textBox; //Set the active line view to this character's

        SetEmote(emote);
    }
}
