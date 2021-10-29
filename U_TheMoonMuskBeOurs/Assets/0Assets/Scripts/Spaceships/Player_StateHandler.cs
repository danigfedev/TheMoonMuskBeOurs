using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_StateHandler : MonoBehaviour, IStateHandler
{
    [SerializeField] Transform playerShip;
    [SerializeField] Transform shieldPrefab;
    [SerializeField] Transform shieldSpawnPos;

    private Transform shieldInstance;

    void Awake()
    {
        InitializeShield();
    }

    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);

        if (other.tag == TagList.bulletPlayerTag) return;

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

    private void HandleDamage()
    {

    }

    void IStateHandler.HandleDamage(float damage)
    {
        throw new System.NotImplementedException();
    }

    public void HandleHealing(float health)
    {
        throw new System.NotImplementedException();
    }
}
