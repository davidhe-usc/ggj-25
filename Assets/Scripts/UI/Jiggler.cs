using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Jiggler : MonoBehaviour
{
    [SerializeField]
    float jiggleDistance;
    [SerializeField]
    float jiggleSpeed;
    [SerializeField]
    float snapMargin;

    RectTransform rectTransform;
    Vector2 homePosition;
    Vector2 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(DelayedStart());
        Debug.Log(Mathf.Cos(Mathf.Deg2Rad * 30));
        Debug.Log(Mathf.Cos(Mathf.Deg2Rad * 150));
        Debug.Log(Mathf.Cos(Mathf.Deg2Rad * 210));
        Debug.Log(Mathf.Cos(Mathf.Deg2Rad * 330));
    }

    // Update is called once per frame
    void Update()
    {
        if((targetPosition.x - rectTransform.localPosition.x <= snapMargin) && (targetPosition.y - rectTransform.localPosition.y <= snapMargin))
        {
            targetPosition = new Vector2(homePosition.x + Random.Range(jiggleDistance * -1, jiggleDistance), homePosition.y + Random.Range(jiggleDistance * -1, jiggleDistance));
            Debug.Log("snapped, new target of " + targetPosition);
            //Debug.Log((targetPosition.x - rectTransform.localPosition.x) + ", " + (targetPosition.y - rectTransform.localPosition.y));
        }
        if(targetPosition.x == rectTransform.localPosition.x) //Avoid divide by 0 error
        {
            targetPosition = new Vector2(targetPosition.x + 0.01f, targetPosition.y);
        }
        float angle = Mathf.Tan((targetPosition.y - rectTransform.localPosition.y) / (targetPosition.x - rectTransform.localPosition.x));
        if (targetPosition.x < rectTransform.localPosition.x)
        {
            angle += Mathf.Deg2Rad * 180;
        }
        float xChange = Mathf.Cos(angle) * jiggleSpeed * Time.deltaTime;
        float yChange = Mathf.Sin(angle) * jiggleSpeed * Time.deltaTime;
        Debug.Log(xChange + ", " + yChange);
        //float xChange = (1 / Mathf.Atan(angle)) * (targetPosition.y - rectTransform.localPosition.y) * jiggleSpeed * Time.deltaTime;
        //float yChange = Mathf.Atan(angle) * (targetPosition.x - rectTransform.localPosition.x) * jiggleSpeed * Time.deltaTime;
        rectTransform.localPosition = new Vector2(rectTransform.localPosition.x + xChange, rectTransform.localPosition.y + yChange);
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(0.01f);
        homePosition = rectTransform.localPosition;
        targetPosition = homePosition;
    }
}
