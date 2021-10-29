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
                int shieldDuration = other.gameObject.GetComponent<PowerUpHandler>().GetPowerUpAmount();
                EnableShield(shieldDuration);
                break;
            case TagList.PU_healthTag:
                int healthAmount = other.gameObject.GetComponent<PowerUpHandler>().GetPowerUpAmount();
                HandleHealing(healthAmount);
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

    private void EnableShield(int shieldDuration)
    {
        shieldInstance.gameObject.SetActive(true);
        shieldInstance.GetComponent<Shield>().EnableShield(shieldDuration);
    }


    protected override void HandleDamage(float damage)
    {
        totalHealth -= damage;

        Debug.Log("Damage taken: " + totalHealth);

        //Edit material
        if(totalHealth <= 0)
        {
            //GameOver!
        }
    }

    protected override void HandleHealing(float health)
    {
        totalHealth += health;

        Debug.Log("Healing: " + totalHealth);

        if (totalHealth > maxHealth) totalHealth = maxHealth;

        //Edit material
    }
}
