using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class IntEventSO_Listener : MonoBehaviour
{
    [System.Serializable]
    public class intUnityEvent : UnityEvent<int> { }

    public IntEventSO eventSO;
    public intUnityEvent response;

    void OnEnable()
    {
        eventSO.AddListener(this);
    }

    void OnDisable()
    {
        eventSO.RemoveListener(this);
    }

    public void RaiseEvent(int n)
    {
        response.Invoke(n);
    }
}
