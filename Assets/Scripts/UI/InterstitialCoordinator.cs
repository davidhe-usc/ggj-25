using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterstitialCoordinator : MonoBehaviour
{
    [SerializeField]
    InterstitialProgressTracker progressTracker;
    [SerializeField]
    DayCounter dayText;
    [SerializeField]
    ImageFader fader;
    [SerializeField]
    SceneChanger sceneChanger;
    [SerializeField]
    float preStartTime;
    [SerializeField]
    float startTime;
    [SerializeField]
    float middleTime;
    [SerializeField]
    float endTime;

    bool skippable = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForStartToFinish());
    }

    // Update is called once per frame
    void Update()
    {
        if (skippable == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                skippable = false;
                EndInterstitial();
            }
        }
    }

    

    public void EndInterstitial()
    {
        fader.FadeIn();
        sceneChanger.DelayedLoadScene(endTime);
    }

    IEnumerator WaitForStartToFinish()
    {
        progressTracker.RepositionInterstitialPortraits();
        dayText.UpdateText();
        yield return new WaitForSeconds(preStartTime);
        fader.FadeOut();
        yield return new WaitForSeconds(startTime);
        skippable = true;
        StartCoroutine(WaitForMiddle());
    }
    IEnumerator WaitForMiddle()
    {
        yield return new WaitForSeconds(middleTime);
        if (skippable == true)
        {
            skippable = false;
            EndInterstitial();
        }
    }
}
