using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BasicBullet : Bullet
{
    private Rigidbody bulletRB;

    void Awake()
    {
        bulletRB = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Move();
    }


    public override void Move()
    {
        //TODO review direction,make it relative to spaceship?
        //Vector3 bulletDirectionV3 = (bulletDirection == BulletDirections.BOTTOM_TOP) ? Vector3.up : Vector3.down;

        Vector3 bulletDirectionV3 = transform.up;
        bulletRB.velocity = bulletDirectionV3 * base.bulletSpeed;
    }
}
