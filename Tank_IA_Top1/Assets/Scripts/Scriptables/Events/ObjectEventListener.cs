using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectEventListener : MonoBehaviour
{
    [SerializeField]
    private ObjectEvent _event;

    [SerializeField]
    private UnityEvent<GameObject> _onEventRaised;

    public void OnEventRaised(GameObject i)
    {
        _onEventRaised.Invoke(i);
    }

    private void OnEnable()
    {
        _event.RegisterListener(this);
    }

    private void OnDisable()
    {
        _event.UnregisterListener(this);
    }
}