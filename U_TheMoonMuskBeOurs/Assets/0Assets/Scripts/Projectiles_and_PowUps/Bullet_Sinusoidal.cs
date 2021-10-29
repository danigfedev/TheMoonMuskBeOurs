using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Sinusoidal : Bullet
{
    [SerializeField] AnimationCurve curve;
    [SerializeField] float amplitude = 1;
    [SerializeField] float frequency = 1;
    [SerializeField] float forwardSpeed = 1;
    
    private float curveEndTime = 0;

    protected override void Start()
    {
        bulletRB.velocity = transform.up * amplitude;
        bulletRB.useGravity = false;

        int curveKeyCount = curve.keys.Length;
        curveEndTime = curve.keys[curveKeyCount - 1].time;
        Debug.Log(string.Format("Curve end time: {0}", curveEndTime));
    }

    float time = 0;
    void FixedUpdate()
    {
        Move();
    }


    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag == TagList.playerTag
            || other.tag == TagList.shieldTag)
            StartCoroutine(Destroy());
    }


    protected override void Move()
    {

        //IDEA: calculating position and applying it to Rigidbody.MovePosition();

        Vector3 newVelocity = bulletRB.velocity;

        time += frequency * Time.fixedDeltaTime;
        if (time > curveEndTime) time = 0;
        newVelocity.x = amplitude * curve.Evaluate(time);
        newVelocity.y = forwardSpeed;
        bulletRB.velocity = newVelocity;
    }

}
