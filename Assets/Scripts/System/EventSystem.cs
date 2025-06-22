using System.Collections.Generic;
using UnityEngine.Events;

public class EventSystem : Singleton<EventSystem>
{
    private Dictionary<string, UnityEvent> eventDictionary = new();



    public void RegisterListener(string eventName, UnityAction listener)
    {
        UnityEvent unityEvent = null;
        if (!eventDictionary.ContainsKey(eventName))
        {
            unityEvent = new UnityEvent();
            unityEvent.AddListener(listener);
            eventDictionary.Add(eventName, unityEvent);
        }
        else
        {
            unityEvent.AddListener(listener);
        }

    }

    public void UnregisterListener(string eventName, UnityAction listener)
    {
        UnityEvent unityEvent = null;
        if (eventDictionary.ContainsKey(eventName))
        {
            unityEvent.RemoveListener(listener);
        }

    }

    public void TriggerEvent(string eventName)
    {
        UnityEvent unityEvent = null;
        if (eventDictionary.ContainsKey(eventName))
        {
            unityEvent.Invoke();
        }

    }
}
