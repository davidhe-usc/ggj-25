//A general use script to make calling events from animations easier.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventCaller : MonoBehaviour
{
    [SerializeField]
    UnityEvent[] events;
    
    public void CallEvent(int eventIndex)
    {
        events[eventIndex].Invoke();
    }
}
