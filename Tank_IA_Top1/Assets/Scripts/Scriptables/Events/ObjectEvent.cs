using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/Object event")]
public class ObjectEvent : ScriptableObject
{
    [SerializeField]
    private List<ObjectEventListener> _listeners;

    public void Raise(GameObject GO)
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i]?.OnEventRaised(GO);
        }
    }

    public void RegisterListener(ObjectEventListener listener)
    {
        _listeners.Add(listener);
    }

    public void UnregisterListener(ObjectEventListener listener)
    {
        _listeners.Remove(listener);
    }
}