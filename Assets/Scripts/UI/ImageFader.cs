using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFader : MonoBehaviour
{
    [SerializeField]
    Image UIImage;
    [SerializeField]
    float FadeSpeed;
    bool fading = false;
    bool targetOpaque = true;

    void Start()
    {

    }
    void Update()
    {
        if (fading == true)
        {
            if (targetOpaque == true)
            {
                if (UIImage.color.a >= 1)
                {
                    fading = false;
                    UIImage.color = new Color(UIImage.color.r, UIImage.color.g, UIImage.color.b, 1);
                } else
                {
                    UIImage.color = new Color(UIImage.color.r, UIImage.color.g, UIImage.color.b, UIImage.color.a + Time.deltaTime * FadeSpeed);
                }
            } else
            {
                if (UIImage.color.a <= 0)
                {
                    fading = false;
                    UIImage.color = new Color(UIImage.color.r, UIImage.color.g, UIImage.color.b, 0);
                }
                else
                {
                    UIImage.color = new Color(UIImage.color.r, UIImage.color.g, UIImage.color.b, UIImage.color.a - Time.deltaTime * FadeSpeed);
                }
            }
        }
    }
    public void FadeOut()
    {
        targetOpaque = false;
        fading = true;
    }

    public void FadeIn()
    {
        targetOpaque = true;
        fading = true;
    }
}
