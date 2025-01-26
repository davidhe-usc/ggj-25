using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DayCounter : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI dayMessage;
    [SerializeField]
    TextMeshProUGUI subtitleText;
    [SerializeField]
    string startOfDayMessage = "Day ";
    [SerializeField]
    int dayNumber;
    [SerializeField]
    string subtitle = "";

    void Start()
    {
        UpdateText();
    }
    
    public void SetDayNumber(int newDayNumber)
    {
        dayNumber = newDayNumber;
    }
    
    public void IncrementDayNumber()
    {
        dayNumber++;
    }

    public void SetSubtitle(string newSubtitle)
    {
        subtitle = newSubtitle;
    }

    public void UpdateText()
    {
        dayMessage.text = startOfDayMessage + dayNumber;
        subtitleText.text = subtitle;
    }
}
