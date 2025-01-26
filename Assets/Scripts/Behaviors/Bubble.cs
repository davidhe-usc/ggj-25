using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    private BubbleManager manager;
    private Collider2D mainTextBox; //Nettle's text box, used to determine collision
    private Rigidbody2D rb;

    public TextMeshPro bubbleText;
    public SpriteRenderer bubbleTextBox;
    public SpriteRenderer bubbleSigil;

    public string id;

    Vector3[] OtherBubbles;
    float lifetime = 0;
    bool locked = false; //Whether the bubble has locked in its position
    bool tooClose = false; //Whether the bubble is too close to the text box and character sprites

    public float startVelocity = 1f; //The magnitude of the bubble's starting velocity
    public float minLifetime = 3f; //The minimum time between the bubble spawning and being able to brake

    private string sigilLetter;
    public Sprite[] possibleSigils;
    public string[] sigilLetterCodes;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Collider2D col = GetComponent<Collider2D>();

        Vector2 push = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        push.Normalize();

        rb.velocity = push;
    }

    // Update is called once per frame
    void Update()
    {

        if (!locked && !tooClose && lifetime > 3f)
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
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn() //Fades in the bubble's text box
    {
        float alpha = bubbleTextBox.color.a;
        while(alpha < 1f)
        {
            alpha += Time.deltaTime;
            bubbleText.alpha = alpha;
            Color c = bubbleTextBox.color;
            c.a = alpha;
            bubbleTextBox.color = c;
            yield return null;
        }
    }

    public void Setup(BubbleManager m, Collider2D t, string l, string s)
    {
        manager = m;
        mainTextBox = t;
        id = l;
        sigilLetter = s;

        //Once we have a complete list of bubble lines, set it up so that we can get the text line from the string identifier.
        bubbleText.text = id;
        AssignSigil();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Respawn") //Placeholder tag
            tooClose = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Respawn") //Placeholder tag
            tooClose = false;
    }

    private void AssignSigil()
    {
        int sigilIndex = 0;
        bool sigilFound = false;
        for (int i = 0; i < sigilLetterCodes.Length && sigilFound == false; i++)
        {
            if(sigilLetter.Equals(sigilLetterCodes[i]))
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
}
