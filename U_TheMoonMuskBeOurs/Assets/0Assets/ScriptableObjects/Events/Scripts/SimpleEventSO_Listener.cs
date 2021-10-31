using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleEventSO_Listener : MonoBehaviour
{
    public SimpleEventSO eventSO;
    public UnityEvent response;

    void OnEnable()
    {
        eventSO.AddListener(this);
    }

    void OnDisable()
    {
        eventSO.RemoveListener(this);
    }

    public void RaiseEvent()
    {
        response.Invoke();
    }
}
