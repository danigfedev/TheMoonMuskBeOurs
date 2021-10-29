using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Smile : Bullet
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag == TagList.playerTag
            || other.tag == TagList.shieldTag)
            StartCoroutine(Destroy());
    }
}
