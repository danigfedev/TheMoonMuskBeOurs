using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Box : Bullet
{
    protected override void OnTriggerEnter(Collider other)
    {
        //Debug.LogWarning(string.Format("Name: {0} | Tag: {1}", other.name, other.tag));

        if (other.tag == TagList.bulletPlayerTag
            || other.tag == TagList.playerTag
            || other.tag == TagList.shieldTag)
            StartCoroutine(Destroy());
    }
}
