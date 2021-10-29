using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PowerUpHandler : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody powUpRB;

    private void Awake() => powUpRB = GetComponent<Rigidbody>();

    private void Start() => Move();

    private void OnTriggerEnter(Collider other)
    {
        //TODO Reset position to pool's root
        gameObject.SetActive(false);
    }

    public void Move()
    {
        Vector3 bulletDirectionV3 = -transform.up;
        powUpRB.velocity = bulletDirectionV3 * speed;/* base.bulletSpeed*/
    }

}
