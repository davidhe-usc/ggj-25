using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterstitialProgressTracker : MonoBehaviour
{
    [SerializeField]
    int trackLength;
    [SerializeField]
    RectTransform firstTick;
    [SerializeField]
    RectTransform lastTick;
    [SerializeField]
    RectTransform playerPortrait;
    [SerializeField]
    RectTransform friendPortrait;
    [SerializeField]
    int minScore;
    [SerializeField]
    int maxScore;
    [SerializeField]
    RectTransform playerStartTick;
    [SerializeField]
    RectTransform friendStartTick;
    
    [SerializeField]
    int tempTestScore;

    float stepSize;
    int scoreOffset = 0; //Can change this if we skew data based on endings achieved.

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitThenReposition());
    }

    public void RepositionInterstitialPortraits()
    {
        int scoreToUse = tempTestScore + scoreOffset;
        if (scoreToUse < minScore)
        {
            scoreToUse = minScore;
        } else if (scoreToUse > maxScore)
        {
            scoreToUse = maxScore;
        }
        playerPortrait.localPosition = playerStartTick.localPosition;
        friendPortrait.localPosition = friendStartTick.localPosition;
        playerPortrait.localPosition = new Vector2(playerPortrait.localPosition.x + (stepSize * scoreToUse * -1), playerPortrait.localPosition.y);
        friendPortrait.localPosition = new Vector2(friendPortrait.localPosition.x + (stepSize * scoreToUse), friendPortrait.localPosition.y);
    }

    IEnumerator WaitThenReposition()
    {
        yield return new WaitForSeconds(0.01f);
        stepSize = (lastTick.anchoredPosition.x - firstTick.anchoredPosition.x) / trackLength;
        RepositionInterstitialPortraits();
    }
}
