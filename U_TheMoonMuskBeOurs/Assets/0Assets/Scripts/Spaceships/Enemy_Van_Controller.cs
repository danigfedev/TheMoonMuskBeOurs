using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Van_Controller : EnemyController_Base
{
    [SerializeField] float angularSpeed;
    //[SerializeField] float linearSpeed = 1;
    private Vector3 pivotPoint;
    private Vector3 linearStartPosition;
    private Vector3 linearEndPosition;

    public override void Start()
    {
        Move();
    }

    public void FixedUpdate()
    {
        Move();
    }

    public void SetPivotPoint(Vector3 pivot) => pivotPoint = pivot;

    public void SetMotionParameters(Vector3 start, Vector3 end, Vector3 pivot)
    {
        linearStartPosition = start;
        linearEndPosition = end;
        pivotPoint = pivot;
    }

    float t = 0;
    public override void Move()
    {

        //Linear movement:
        if(t<=1.1f/*transform.position != linearEndPosition*/) //Making sure 1 is reached
        {
            Vector3 newPosition = Vector3.Lerp(linearStartPosition, linearEndPosition, t);
            enemyRigidbody.MovePosition(newPosition);

            t += Time.fixedDeltaTime * speed;
            return;
        }

        //Rotational movement:
        Vector3 v = transform.position - pivotPoint;
        float mod = v.magnitude;
        v.Normalize();

        v = Quaternion.AngleAxis(Time.deltaTime * angularSpeed, -Vector3.forward) * v;
        enemyRigidbody.MovePosition(pivotPoint+v * mod);
    }
}
