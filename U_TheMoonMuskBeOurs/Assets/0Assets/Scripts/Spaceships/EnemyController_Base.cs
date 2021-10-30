using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class EnemyController_Base : MonoBehaviour
{
    [SerializeField] protected float speed;
    protected Rigidbody enemyRigidbody;

    public virtual void Awake()
    {
        enemyRigidbody = GetComponent<Rigidbody>();
    }

    public virtual void Start()
    {
        Move();
    }

    public virtual void Move()
    {
        enemyRigidbody.velocity = transform.up * speed;
    }

}
