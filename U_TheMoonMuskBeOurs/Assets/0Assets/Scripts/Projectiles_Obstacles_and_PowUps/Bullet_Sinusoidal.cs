using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Sinusoidal : Bullet
{
    public enum SinusoidalDirection
    {
        VERTICAL = 0,
        HORIZONTAL
    }

    [SerializeField] AnimationCurve curve;
    [SerializeField] float amplitude = 1;
    [SerializeField] float frequency = 1;
    [SerializeField] float forwardSpeed = 1;
    [SerializeField] SinusoidalDirection direction = SinusoidalDirection.VERTICAL;

    private float curveEndTime = 0;
    private bool useInverseFwdSpeed = false;

    private void OnDisable() => useInverseFwdSpeed = false;

    protected override void Start()
    {
        bulletRB.velocity = transform.up * amplitude;
        bulletRB.useGravity = false;

        int curveKeyCount = curve.keys.Length;
        curveEndTime = curve.keys[curveKeyCount - 1].time;
        //Debug.Log(string.Format("Curve end time: {0}", curveEndTime));
    }

    float time = 0;
    void FixedUpdate()
    {
        Move();
    }


    public void UseNegativeSpeed() => useInverseFwdSpeed = true;

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
        
        float sinSpeed= amplitude * curve.Evaluate(time);
        //float linearSpeed = forwardSpeed;
        float linearSpeed = useInverseFwdSpeed ?-forwardSpeed : forwardSpeed;

        if (direction == SinusoidalDirection.VERTICAL)
        {
            newVelocity.x = sinSpeed;
            newVelocity.y = linearSpeed;
        }
        else
        {
            newVelocity.x = linearSpeed;
            newVelocity.y = sinSpeed;
        }

        bulletRB.velocity = newVelocity;
    }

}
