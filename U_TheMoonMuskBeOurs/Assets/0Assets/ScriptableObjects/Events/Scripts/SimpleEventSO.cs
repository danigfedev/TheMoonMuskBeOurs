using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A Unity event handled via Scriptable Object that takes no arguments
/// </summary>
[CreateAssetMenu(fileName = "SimpleEventSO", menuName = "ScriptableObjects/Events/SimpleEventSO", order = 1)]
public class SimpleEventSO : ScriptableObject
{
    private List<SimpleEventSO_Listener> listenerList = new List<SimpleEventSO_Listener>();

    public void AddListener(SimpleEventSO_Listener listener)
    {
        listenerList.Add(listener);
    }

    public void RemoveListener(SimpleEventSO_Listener listener)
    {
        listenerList.Remove(listener);
    }

    public void RaiseEvent()
    {

        //Looping listener list backwards in case any object destroys itself on event raise
        for (int i = listenerList.Count-1; i >=0; i--)
        {
            listenerList[i].RaiseEvent();
        }
    }
}
