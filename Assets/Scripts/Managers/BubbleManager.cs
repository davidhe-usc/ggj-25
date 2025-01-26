using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

public class BubbleManager : MonoBehaviour
{
    public GameObject bubblePrefab; //The prefab to use for spawned bubbles
    public Collider2D textBoxCollider; //The dummy collider meant to represent where the text box would be in worldspace

    private List<Bubble> bubbles;

    public float bubbleDistanceLimit = 4f; //The minimum distance between each bubble

    public Transform[] spawnPoints; //Where the bubbles are allowed to spawn

    // Start is called before the first frame update
    void Start()
    {
        bubbles = new List<Bubble>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [YarnCommand("SpawnBubble")]
    public void SpawnBubble(string bubbleLine)
    {
        Transform p;
        do
        {
            p = spawnPoints[Random.Range(0, spawnPoints.Length)];
        } while (!CheckBubblePositions(p.position));

        Bubble b = Instantiate(bubblePrefab, p.position, Quaternion.identity).GetComponent<Bubble>();
        bubbles.Add(b);
        b.Setup(this, textBoxCollider, bubbleLine);
    }

    public void BubbleChosen(string bubble, bool popped)
    {
        //Do something when the bubble is popped or frozen

        bubbles.Clear();
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
}
