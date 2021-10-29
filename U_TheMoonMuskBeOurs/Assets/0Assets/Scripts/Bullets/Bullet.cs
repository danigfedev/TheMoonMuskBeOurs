using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public float bulletDamage;
    public float bulletSpeed;
    public float GetBulletDamage()
    {
        return bulletDamage;
    }
    public abstract void Move(/*float speed*/);
}
