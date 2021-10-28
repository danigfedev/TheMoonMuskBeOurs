using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHandler : MonoBehaviour
{
    [SerializeField] Transform playerShip;
    [SerializeField] Transform shieldPrefab;
    [SerializeField] Transform shieldSpawnPos;

    private Transform shieldInstance;

    void Awake()
    {
        InitializeShield();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == TagList.bulletTag) return;

        switch (other.tag)
        {
            case TagList.PU_shieldTag:
                EnableShield();
                break;
        }
    }

    private void InitializeShield()
    {
        shieldInstance = Instantiate(shieldPrefab);
        shieldInstance.gameObject.SetActive(false);
        shieldInstance.position = shieldSpawnPos.position;
        shieldInstance.GetComponent<Shield>().PlayerShip = playerShip;
    }

    private void EnableShield()
    {
        shieldInstance.gameObject.SetActive(true);
    }
}
