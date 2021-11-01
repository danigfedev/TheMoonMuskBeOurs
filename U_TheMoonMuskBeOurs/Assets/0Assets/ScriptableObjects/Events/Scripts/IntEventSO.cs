using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A Unity event handled via Scriptable Object that takes no arguments
/// </summary>
[CreateAssetMenu(fileName = "SimpleEventSO", menuName = "ScriptableObjects/Events/IntEventSO", order = 2)]
public class IntEventSO : ScriptableObject
{
    private List<IntEventSO_Listener> listenerList = new List<IntEventSO_Listener>();

    public void AddListener(IntEventSO_Listener listener)
    {
        listenerList.Add(listener);
    }

    public void RemoveListener(IntEventSO_Listener listener)
    {
        listenerList.Remove(listener);
    }

    public void RaiseEvent(int n)
    {

        //Looping listener list backwards in case any object destroys itself on event raise
        for (int i = listenerList.Count-1; i >=0; i--)
        {
            listenerList[i].RaiseEvent(n);
        }
    }
}
