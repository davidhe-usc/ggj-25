using Febucci.UI;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class BubbleManager : MonoBehaviour
{
    public GameObject bubblePrefab; //The prefab to use for spawned bubbles
    public Collider2D textBoxCollider; //The dummy collider meant to represent where the text box would be in worldspace

    private List<Bubble> bubbles;

    public float bubbleDistanceLimit = 4f; //The minimum distance between each bubble
    public float bubbleDelay = 1f; //The time in seconds between the line starting and the bubble appearing

    public Transform[] spawnPoints; //Where the bubbles are allowed to spawn

    DialogueRunner dr;
    CaptureManager cm;

    private Bubble activeBubble; //The bubble being selected for the freeze or pop choice
    private string activeLabel; //The selected bubble's label
    private bool popInputReady = false; //Whether the manager responds to pop/freeze button presses

    public CanvasGroup choiceMenu;
    public TextMeshProUGUI bubbleChoiceText;

    private string afterNode; //The node that plays after the current freeze/pop choice

    public Texture2D defaultCursor;
    public Texture2D wandCursor;

    public GameObject filter;

    // Start is called before the first frame update
    void Start()
    {
        dr = FindObjectOfType<DialogueRunner>();
        cm = FindObjectOfType<CaptureManager>();
        bubbles = new List<Bubble>();
        choiceMenu.alpha = 0f;
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [YarnCommand("SpawnBubble")]
    public void SpawnBubble(string bubbleLine, string sigilLetter) //Old version that spawns the bubble at the start of a yarn line
    {
        StartCoroutine(BubbleDelay(bubbleLine, sigilLetter));
    }

    IEnumerator BubbleDelay(string bubbleLine, string sigilLetter)
    {
        yield return new WaitForSeconds(0.5f);

        Bubble b;

        if (bubbleLine.Equals("Intro"))
        {
            b = Instantiate(bubblePrefab, new Vector3(0,0f,0), Quaternion.identity).GetComponent<Bubble>();
            cm.canCapture = true;
            Cursor.SetCursor(wandCursor, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Transform p;
            do
            {
                p = spawnPoints[Random.Range(0, spawnPoints.Length)];
            } while (!CheckBubblePositions(p.position));

            b = Instantiate(bubblePrefab, p.position, Quaternion.identity).GetComponent<Bubble>();
            bubbles.Add(b);
        }

        b.Setup(this, textBoxCollider, bubbleLine, sigilLetter);
    }

    [YarnCommand("CaptureReady")]
    public void BubblesReady()
    {
        cm.canCapture = true;
        Cursor.SetCursor(wandCursor, Vector2.zero, CursorMode.Auto);
        foreach (Bubble b in bubbles)
            b.AllReady();
        StartCoroutine(FilterFade());
    }

    IEnumerator FilterFade()
    {
        SpriteRenderer filterSR = filter.GetComponent<SpriteRenderer>();
        Color alpha = filterSR.color;

        while(alpha.a < 1f)
        {
            alpha.a += Time.deltaTime*3f;
            filterSR.color = alpha;
            yield return null;
        }
    }

    IEnumerator FilterFadeOut()
    {
        SpriteRenderer filterSR = filter.GetComponent<SpriteRenderer>();
        Color alpha = filterSR.color;

        while (alpha.a > 0f)
        {
            alpha.a -= Time.deltaTime * 3f;
            filterSR.color = alpha;
            yield return null;
        }
    }

    public void BubbleChosen(string id, string l, Bubble bubble) //Clean up the rest of the bubbles once one is chosen, set up the dialogue choices
    {
        cm.canCapture = false;

        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);

        activeBubble = bubble;

        foreach(Bubble b in bubbles)
        {
            if (b.id != id)
            {
                //call the fade animation
                //check if the id matches and then do a different effect based on the popped bool
                b.Unselected();
                GameObject.Destroy(b.gameObject, 1f);
            }
        }

        bubbleChoiceText.text = l;

        StartCoroutine(FadeChoices(true));
    }

    IEnumerator FadeChoices(bool fadeIn)
    {
        if(fadeIn)
        {
            while(choiceMenu.alpha < 1f)
            {
                choiceMenu.alpha += Time.deltaTime;
                yield return null;
            }
            popInputReady = true;
        }
        else
        {
            while (choiceMenu.alpha > 0f)
            {
                choiceMenu.alpha -= Time.deltaTime*2;
                yield return null;
            }
        }
    }

    public void ActivePop(bool pop) //Call this with the buttons
    {
        if (popInputReady)
        {
            popInputReady = false;
            StartCoroutine(FadeChoices(false));

            var nodes = activeBubble.Pop(pop);

            dr.StartDialogue(nodes.Result);

            afterNode = nodes.After;

            bubbles.Clear();

            //filter.SetActive(false);
            StartCoroutine(FilterFadeOut());
        }
    }

    public bool CheckBubblePositions(string t, Vector3 pos) //Returns true if the bubble with id t is farther than the bubble distance limit to all other bubbles
    {
        foreach (Bubble b in bubbles)
        {
            if (b.id != t && Vector3.Distance(b.transform.position, pos) < bubbleDistanceLimit)
            {
                return false;
            }
        }

        return true;
    }

    public bool CheckBubblePositions(Vector3 pos) //Checks all exisiting bubbles, used for spawning them in
    {
        foreach (Bubble b in bubbles)
        {
            if (Vector3.Distance(b.transform.position, pos) < bubbleDistanceLimit)
            {
                return false;
            }
        }

        return true;
    }

    [YarnCommand("NextNode")]
    public void NextNode()
    {
        StartCoroutine(QueueNode(afterNode));
    }

    IEnumerator QueueNode(string node)
    {
        while(dr.IsDialogueRunning)
        {
            yield return null;
        }
        dr.StartDialogue(node);
    }

    [YarnCommand("FiveFive")]
    public void SetFive()
    {
        afterNode = "Conversation5-6";
        if (PlayerPrefs.GetInt("SelflessEndAchieved") == 1)
        {
            StartCoroutine(QueueNode("Conversation5-5Selfless"));
        }
        else if (PlayerPrefs.GetInt("SelfishEndAchieved") == 1)
        {
            StartCoroutine(QueueNode("Conversation5-5Selfish"));
        }
        else
        {
            StartCoroutine(QueueNode("Conversation5-6"));
        }
    }

    public void EndIntro()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        cm.canCapture = false;
        IntroManager im = FindObjectOfType<IntroManager>();
        im.EndIntro();
    }
}
