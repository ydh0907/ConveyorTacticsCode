using System;
using System.Collections.Generic;
using UnityEngine;
using MonoBehaviour = UnityEngine.MonoBehaviour;

[CreateAssetMenu(menuName = "SO/Event/EventListSO")]
public class EventListSO : ScriptableObject
{
    public List<EventSO> Events;

    public void Init()
    {
        foreach (var eventSO in Events)
        {
            eventSO.Init();
        }
    }

    public void StartEvent(int index)
    {
        Events[index].StartEvent();
    }
}