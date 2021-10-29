using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_StateHandler : StateHandler_Base
{
    [SerializeField] Transform playerShip;
    [SerializeField] Transform shieldPrefab;
    [SerializeField] Transform shieldSpawnPos;

    private Transform shieldInstance;
    

    public override void Awake()
    {
        InitializeShield();
        base.Awake();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case TagList.PU_shieldTag:
                EnableShield();
                break;

            case TagList.enemyTag:
                HandleDamage(2);
                break;
            case TagList.bulletBoxTag: //Enemy bullet
                float damage = other.GetComponentInParent<Bullet>().GetBulletDamage();
                HandleDamage(damage);
                break;
        }

        //other.gameObject.SetActive(false);
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


    protected override void HandleDamage(float damage)
    {
        totalHealth -= damage;
        //Edit material
        if(totalHealth <= 0)
        {
            //GameOver!
        }
    }

    protected override void HandleHealing(float health)
    {
        totalHealth += health;
        if (totalHealth > maxHealth) totalHealth = maxHealth;

        //Edit material
    }
}
