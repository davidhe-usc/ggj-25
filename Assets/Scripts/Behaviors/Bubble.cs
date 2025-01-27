using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    private BubbleManager manager;
    private Collider2D mainTextBox; //Nettle's text box, used to determine collision
    private Rigidbody2D rb;
    private Animator animator;

    public TextMeshPro bubbleText;
    public SpriteRenderer bubbleTextBox;
    public SpriteRenderer bubbleSigil;

    public string id;
    private string popNode;
    private string freezeNode;
    private string nodeAfter;

    Vector3[] OtherBubbles;
    float lifetime = 0;
    bool locked = false; //Whether the bubble has locked in its position
    bool tooClose = false; //Whether the bubble is too close to the text box and character sprites

    public float startVelocity = 1f; //The magnitude of the bubble's starting velocity
    public float minLifetime = 3f; //The minimum time between the bubble spawning and being able to brake

    private string sigilLetter;
    public Sprite[] possibleSigils;
    public string[] sigilLetterCodes;

    public int threshold = 5;
    private int selectCounter;

    private bool intro = false;

    public ParticleSystem popEffect;
    public GameObject bubbleSprite;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        Collider2D col = GetComponent<Collider2D>();

        if (!intro)
        {

            Vector2 push = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            push.Normalize();

            rb.velocity = push;
        }

        selectCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (intro)
        {
            locked = true;
            StartCoroutine(Brake());
        }
        else if (!locked && !tooClose && lifetime > 3f)
        {
            if(manager.CheckBubblePositions(id, transform.position))
            {
                StartCoroutine(Brake());
                locked = true;
            }
        }
        else
        {
            lifetime += Time.deltaTime;
        }
        
    }

    IEnumerator Brake()
    {
        while (rb.velocity.magnitude > 0.01f)
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, 0.005f);
            yield return null;
        }
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        if (!intro)
        {
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeIn() //Fades in the bubble's text box
    {
        /*float alpha = bubbleTextBox.color.a;
        while(alpha < 1f)
        {
            alpha += Time.deltaTime;
            bubbleText.alpha = alpha;
            Color c = bubbleTextBox.color;
            c.a = alpha;
            bubbleTextBox.color = c;
            yield return null;
        }*/
        yield return null;
    }

    public void Setup(BubbleManager m, Collider2D t, string l, string s)
    {
        manager = m;
        mainTextBox = t;
        id = l;
        sigilLetter = s;

        if (id.Equals("Intro"))
        {
            intro = true;
            bubbleText.alpha = 0f;
            Color c = bubbleTextBox.color;
            c.a = 0;
            bubbleTextBox.color = c;
        }
        else
        {
            //Once we have a complete list of bubble lines, set it up so that we can get the text line from the string identifier.
            SetText();
        }
        AssignSigil();
    }

    public void SelectBubble() //Select the bubble to move to the center of the screen
    {
        if (intro)
        {
            manager.EndIntro();
        }
        else
        {
            manager.BubbleChosen(id, bubbleText.text, this);
            StartCoroutine(MoveToSelect());
        }
    }

    IEnumerator MoveToSelect()
    {
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        while (Vector3.Distance(transform.position, Vector3.zero) > 0.05f)
        {
            rb.MovePosition(Vector2.Lerp(transform.position, Vector3.zero, 0.05f));
            yield return null;
        }
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    }
    private void AssignSigil()
    {
        int sigilIndex = 0;
        bool sigilFound = false;
        for (int i = 0; i < sigilLetterCodes.Length && sigilFound == false; i++)
        {
            if (sigilLetter.Equals(sigilLetterCodes[i]))
            {
                sigilIndex = i;
                sigilFound = true;
            }
        }
        if (sigilFound == false)
        {
            Debug.LogError("Sigil letter " + sigilLetter + " not found!  Default sigil used!");
        }
        bubbleSigil.sprite = possibleSigils[sigilIndex];
    }

    public (string Result, string After) Pop(bool pop) //Pops the bubble. Set parameter to true if popped or false if frozen. Return the node to play if it was popped or frozen
    {
        if(pop)
        {
            popEffect.Play();
            bubbleSprite.SetActive(false);
            GameObject.Destroy(this.gameObject, 1f);
            return (popNode, nodeAfter);
        }
        else
        {
            animator.SetBool("Freeze", true);
            GameObject.Destroy(this.gameObject, 1f);
            return (freezeNode, nodeAfter);
        }
    }

    public void FadeAnimation()
    {
        //do the animation to fade
        Destroy(this.gameObject, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Respawn") //Placeholder tag
            tooClose = true;
        if(collision.tag == "Loop")
        {
            var bubbleBounds = this.GetComponent<Collider2D>().bounds;
            bool checkMinBounds = (collision.bounds.min.x <= bubbleBounds.min.x && collision.bounds.min.y <= bubbleBounds.min.y);
            bool checkMaxBounds = (collision.bounds.max.x >= bubbleBounds.max.x && collision.bounds.max.y >= bubbleBounds.max.y);
            if (checkMinBounds && checkMaxBounds)
            {
                selectCounter++;
                if (selectCounter >= threshold)
                    SelectBubble();
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Respawn") //Placeholder tag
            tooClose = false;
    }

    private void setNodes(string id, string next) {
        popNode = "Conversation" + id + "Pop";
        freezeNode = "Conversation" + id + "Freeze";
        nodeAfter = "Conversation" + next;
    }

    public void Unselected()
    {
        popEffect.Play();
        bubbleSprite.SetActive(false);
    }

    private void SetText() //Set the bubble text and next node based on id
    {
        switch(id)
        {
            // ------------ CONVERSATION 1 ---------------
            // Choice 1
            case ("LastTimeIWasHere"):
                bubbleText.text = "Last Time I Was Here";
                setNodes("1-1-1", "1-2");
                break;
            case ("There'sSoMuchToDo"):
                bubbleText.text = "There's So Much To Do";
                setNodes("1-1-2", "1-2");
                break;
            // Choice 2
            case ("Work"):
                bubbleText.text = "Work";
                setNodes("1-2-1", "1-3");
                break;
            case ("School"):
                bubbleText.text = "School";
                setNodes("1-2-2", "1-3");
                break;
            case ("MyNewFriends"):
                bubbleText.text = "My New Friends";
                setNodes("1-2-3", "1-3");
                break;
            // choice 3
            case ("ILiked"):
                bubbleText.text = "I Liked";
                setNodes("1-3-1", "1-4");
                break;
            case ("ALotOfWork"):
                bubbleText.text = "A Lot of Work";
                setNodes("1-3-2", "1-4");
                break;
            case ("HardToManage"):
                bubbleText.text = "Hard to Manage";
                setNodes("1-3-3", "1-4");
                break;
            // ------------ CONVERSATION 2 ---------------
            // Choice 1
            case ("MyFriends"):
                bubbleText.text = "My Friends";
                setNodes("2-1-1", "2-2");
                break;
            case ("Souvenirs"):
                bubbleText.text = "Souvenirs";
                setNodes("2-1-2", "2-2");
                break;
            case ("CoralburgIsSoFar"):
                bubbleText.text = "Coralburg Is So Far";
                setNodes("2-1-3", "2-2");
                break;
            // Choice 2
            case ("Doesn'tThinkIt'sSafe"):
                bubbleText.text = "Doesn't Think It's Safe";
                setNodes("2-2-1", "2-3");
                break;
            case ("Doesn'tLikeMyFriends"):
                bubbleText.text = "Doesn't Like My Friends";
                setNodes("2-2-2", "2-3");
                break;
            case ("BestOpportunities"):
                bubbleText.text = "Best Opportunities";
                setNodes("2-2-3", "2-3");
                break;
            // ------------ CONVERSATION 3 ---------------
            // Choice 1
            case ("MyFriends2"):
                bubbleText.text = "My Friends";
                setNodes("3-1-1", "3-2");
                break;
            case ("FeelSafer"):
                bubbleText.text = "Feel Safer";
                setNodes("3-1-2", "3-2");
                break;
            case ("WalkSomewhereNice"):
                bubbleText.text = "WalkSomewhereNice";
                setNodes("3-1-3", "3-2");
                break;
            // Choice 2
            case ("WhenWeWorkedTogether"):
                bubbleText.text = "When We Worked Together";
                setNodes("3-2-1", "3-3");
                break;
            case ("School2"):
                bubbleText.text = "School";
                setNodes("3-2-2", "3-3");
                break;
            case ("Work2"):
                bubbleText.text = "Work";
                setNodes("3-2-3", "3-3");
                break;
            // Choice 3
            case ("CoopedUp"):
                bubbleText.text = "Cooped Up";
                setNodes("3-3-1", "3-4");
                break;
            case ("StuffIHaveToDo"):
                bubbleText.text = "Stuff I Have To Do";
                setNodes("3-3-2", "3-4");
                break;
            case ("ActuallyRelaxed"):
                bubbleText.text = "Actually Relaxed";
                setNodes("3-3-3", "3-4");
                break;
            // ------------ CONVERSATION 4 ---------------
            // Choice 1
            case ("SpentThisWeekTogether"):
                bubbleText.text = "Spent This Week Together";
                setNodes("4-1-1", "4-2");
                break;
            case ("Haven'tSpoken"):
                bubbleText.text = "Haven't Spoken";
                setNodes("4-1-2", "4-2");
                break;
            case ("We'veChanged"):
                bubbleText.text = "We've Changed";
                setNodes("4-1-3", "4-2");
                break;
            // Choice 2
            case ("SnuckOut"):
                bubbleText.text = "Snuck Out";
                setNodes("4-2-1", "4-3");
                break;
            case ("AwfulCrush"):
                bubbleText.text = "Awful Crush";
                setNodes("4-2-2", "4-3");
                break;
            case ("MatchingEarrings"):
                bubbleText.text = "Matching Earrings";
                setNodes("4-2-3", "4-3");
                break;
            // Choice 3
            case ("TheseBands"):
                bubbleText.text = "These Bands";
                setNodes("4-3-1", "4-4");
                break;
            case ("AllThisStuff"):
                bubbleText.text = "All This Stuff";
                setNodes("4-3-2", "4-4");
                break;
            case ("TheBook"):
                bubbleText.text = "TheBook";
                setNodes("4-3-3", "4-4");
                break;
            // Choice 4
            case ("SuperConvenient"):
                bubbleText.text = "Super Convenient";
                setNodes("4-4-1", "4-5");
                break;
            case ("ThingsWereSlower"):
                bubbleText.text = "When Things Were Slower";
                setNodes("4-4-2", "4-5");
                break;
            case ("IfIStayedHere"):
                bubbleText.text = "If I Stayed Here";
                setNodes("4-4-3", "4-5");
                break;
            // ------------ CONVERSATION 5 ---------------
            // Choice 1
            case ("IMissedYou"):
                bubbleText.text = "I Missed You";
                setNodes("5-1-1", "5-2");
                break;
            case ("TheCafe"):
                bubbleText.text = "The Cafe";
                setNodes("5-1-2", "5-2");
                break;
            case ("StillAPlaceForMe"):
                bubbleText.text = "Still A Place For Me";
                setNodes("5-1-3", "5-2");
                break;
            // Choice 2
            case ("ThisPark"):
                bubbleText.text = "This Park";
                setNodes("5-2-1", "5-3");
                break;
            case ("AsKids"):
                bubbleText.text = "As Kids";
                setNodes("5-2-2", "5-3");
                break;
            case ("HereAllTheTime"):
                bubbleText.text = "Here All The Time";
                setNodes("5-2-3", "5-3");
                break;
            // Choice 3
            case ("NotThatSimple"):
                bubbleText.text = "Not That Simple";
                setNodes("5-3-1", "5-4");
                break;
            case ("ICouldStay"):
                bubbleText.text = "I Could Stay";
                setNodes("5-3-2", "5-4");
                break;
            case ("MissYou"):
                bubbleText.text = "Miss You";
                setNodes("5-3-3", "5-4");
                break;

            default:
                bubbleText.text = "TEST";
                popNode = "Conversation1-1-1Pop";
                freezeNode = "Conversation1-1-1Freeze";
                break;
        }
    }
}
