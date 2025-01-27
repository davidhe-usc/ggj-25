using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Character : MonoBehaviour
{
    private DialogueRunner dr;
    private SpriteRenderer sr;

    public Sprite[] emotionSprites; //List of sprites for the character for a given scene
    public LineView textBox; //The personal text box for this character
    public GameObject mouth;
    public GameObject baseArm;

    private void Awake()
    {
        dr = FindObjectOfType<DialogueRunner>(); //We only got one per scene
        sr = GetComponent<SpriteRenderer>(); //Get this character's renderer
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [YarnCommand("Emote")]
    public void SetEmote(string emote) //Just sets the emote. Can be called directly when we don't want to change the speaker
    {
        //Set emotion sprite. Currently our options are:
        // :) Neutral : 0
        // :O Open : 1
        // :D Happy : 2
        // ^O^ Sheepish : 3
        // ó^ò Worried : 4
        // :( Sad : 5
        // :| TrueNeutral : 6
        // nOn Cheerful : 7


        switch (emote)
        {
            case ("Neutral"):
                sr.sprite = emotionSprites[0];
                break;
            case ("Open"):
                sr.sprite = emotionSprites[1];
                break;
            case ("Happy"):
                sr.sprite = emotionSprites[2];
                break;
            case ("Sheepish"):
                sr.sprite = emotionSprites[3];
                break;
            case ("Worried"):
                sr.sprite = emotionSprites[4];
                break;
            case ("Sad"):
                sr.sprite = emotionSprites[5];
                break;
            case ("TrueNeutral"):
                sr.sprite = emotionSprites[6];
                break;
            case ("Cheerful"):
                sr.sprite = emotionSprites[7];
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
        //dr.dialogueViews[0] = textBox; //Set the active line view to this character'son
        if (textBox != null)
        {
            DialogueViewBase[] box = { textBox };
            dr.SetDialogueViews(box);
        }
        else
        {
            Debug.LogWarning("Issues");
        }


        SetEmote(emote);
    }

    [YarnCommand("Mouth")]
    public void MouthSprite(int open)
    {
        if(open == 1)
        {
            mouth.SetActive(true);
            if(baseArm != null)
                baseArm.SetActive(false);
        }
        else
        {
            mouth.SetActive(false);
            if(baseArm != null)
                baseArm.SetActive(true);
        }
    }
}
