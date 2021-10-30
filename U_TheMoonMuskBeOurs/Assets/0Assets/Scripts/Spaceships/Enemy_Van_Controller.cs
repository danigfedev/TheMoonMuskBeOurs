using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Van_Controller : EnemyController_Base
{
    [SerializeField] float angularSpeed;
    private Vector3 pivotPoint;

    public override void Start()
    {
        Move();
    }

    public void FixedUpdate()
    {
        Move();
    }

    public void SetPivotPoint(Vector3 pivot) => pivotPoint = pivot;

    public override void Move()
    {
        Vector3 v = transform.position - pivotPoint;
        float mod = v.magnitude;
        v.Normalize();

        Debug.Log(v);
        v = Quaternion.AngleAxis(Time.deltaTime * angularSpeed, -Vector3.forward) * v;
        Debug.Log("Rotated: " + v);
        enemyRigidbody.MovePosition(pivotPoint+v * mod);
    }
}
