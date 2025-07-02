using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.TextCore;

public class EventSystem : Singleton<EventSystem>
{
    private readonly Dictionary<string, UnityEvent> _eventDictionary = new();



    public void RegisterListener(string eventName, UnityAction listener)
    {
        if (!_eventDictionary.ContainsKey(eventName))
        {
            UnityEvent unityEvent = new();
            unityEvent.AddListener(listener);
            _eventDictionary.Add(eventName, unityEvent);
        }
        else
        {
            _eventDictionary[eventName].AddListener(listener);
        }

    }

    public void UnregisterListener(string eventName, UnityAction listener)
    {
        if (_eventDictionary.TryGetValue(eventName, out UnityEvent unityEvent))
        {
            unityEvent.RemoveListener(listener);
        }

    }

    public void TriggerEvent(string eventName)
    {
        if (_eventDictionary.TryGetValue(eventName, out UnityEvent unityEvent))
        {
            unityEvent.Invoke();
        }

    }
}
