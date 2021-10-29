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
        string _tag = other.tag;
        if(_tag == TagList.PU_shieldTag)
        {
            int shieldDuration = other.gameObject.GetComponent<PowerUpHandler>().GetPowerUpAmount();
            EnableShield(shieldDuration);
        }
        else if(_tag== TagList.PU_healthTag)
        {
            int healthAmount = other.gameObject.GetComponent<PowerUpHandler>().GetPowerUpAmount();
            HandleHealing(healthAmount);
        }
        else if(_tag== TagList.enemyTag)
        {
            // Collision with enemy.
            float damage = other.gameObject.GetComponentInParent<StateHandler_Base>().GetHitDamage();
            HandleDamage(damage);//hardcoded for now
        }
        else if (_tag.Contains(TagList.bulletPrefix))
        {
            if (_tag != TagList.bulletPlayerTag)
            {
                float damage = other.GetComponentInParent<Bullet>().GetBulletDamage();
                HandleDamage(damage);
            }
        }

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
        //Debug.Log("Damage taken: " + totalHealth);
        //Edit material
        if(totalHealth <= 0)
        {
            //GameOver!
            Debug.LogError("YOU DIED");
        }
    }

    protected override void HandleHealing(float health)
    {
        totalHealth += health;

        if (totalHealth > maxHealth) totalHealth = maxHealth;
        
        Debug.Log("Healing: " + totalHealth);
        //Edit material
    }
}
