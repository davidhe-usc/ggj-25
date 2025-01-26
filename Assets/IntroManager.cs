using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class IntroManager : MonoBehaviour
{
    public BubbleManager bubbleMan;

    public Image fade;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [YarnCommand("IntroBubble")]
    public void IntroBubble()
    {
        bubbleMan.SpawnBubble("Intro", "P");
    }

    public void EndIntro()
    {
        StartCoroutine(EndFade());
    }

    IEnumerator EndFade()
    {
        while(fade.color.a < 1)
        {
            Color alpha = fade.color;
            alpha.a += Time.deltaTime;
            fade.color = alpha;
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        SceneChanger sm = FindObjectOfType<SceneChanger>();
        sm.LoadNextScene();
    }
}
