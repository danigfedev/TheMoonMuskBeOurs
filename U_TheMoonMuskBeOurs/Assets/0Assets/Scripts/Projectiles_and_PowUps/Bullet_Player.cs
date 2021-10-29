using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet_Player : Bullet
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag == TagList.bulletBoxTag
            || other.tag == TagList.enemyTag
            /*|| other.tag == TagList.bossTag*/)
            StartCoroutine(Destroy());
    }
}
