using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PowerUpHandler : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] float speed;
    [Header("PowerUp")]
    [Tooltip("The amount related to what the PowerUp does: \n" +
        "- bullet count for Weapon_PU\n" +
        "- Health for Health_PU and\n" +
        "- Shield duration for shield")]
    [SerializeField] int amount = 0;

    private Rigidbody powUpRB;

    private void Awake() => powUpRB = GetComponent<Rigidbody>();

    private void Start() => Move();

    private void OnTriggerEnter(Collider other)
    {
        //TODO Reset position to pool's root
        gameObject.SetActive(false);
    }

    public int GetPowerUpAmount() => amount;

    public void Move()
    {
        Vector3 bulletDirectionV3 = -transform.up;
        powUpRB.velocity = bulletDirectionV3 * speed;/* base.bulletSpeed*/
    }

}
