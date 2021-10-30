using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController_Bullet : Bullet
{
    protected override void OnTriggerEnter(Collider other)
    {
        //throw new System.NotImplementedException();
        Debug.Log(other.tag);
        if (other.tag == TagList.playerTag)
        {
            Debug.LogWarning("Player hit by cloud");
        }
    }
}
