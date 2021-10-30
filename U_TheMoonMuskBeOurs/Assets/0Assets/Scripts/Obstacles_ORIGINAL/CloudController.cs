using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CloudController : MonoBehaviour, IObstacle
{
    public float speed = 5;
    private Rigidbody cloudRB;

    private void Awake() => cloudRB = GetComponent<Rigidbody>();

    private void Start() => cloudRB.velocity = Vector3.down * speed;

    public void OnTriggerEnter(Collider other)
    {
        OnObstacleTriggerEnter(other);
    }

    public void OnObstacleTriggerEnter(Collider other)
    {
        throw new System.NotImplementedException();
    }
}
