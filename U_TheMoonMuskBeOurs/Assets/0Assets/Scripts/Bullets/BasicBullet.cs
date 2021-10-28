using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BasicBullet : Bullet
{
    //public float bulletSpeed = 1;
    public BulletDirections bulletDirection;
    public enum BulletDirections
    {
        TOP_BOTTOM=0,
        BOTTOM_TOP
    }
    private Rigidbody bulletRB;

    void Awake()
    {
        bulletRB = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Move();
    }


    public override void Move(/*float speed*/)
    {
        //TODO review direction,make it relative to spaceship?
        Vector3 bulletDirectionV3 = (bulletDirection == BulletDirections.BOTTOM_TOP) ? Vector3.up : Vector3.down;
        bulletRB.velocity = bulletDirectionV3 * base.bulletSpeed;
    }
}
