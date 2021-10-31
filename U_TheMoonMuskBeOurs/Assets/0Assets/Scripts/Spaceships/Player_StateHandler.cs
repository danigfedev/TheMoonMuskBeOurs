using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_StateHandler : StateHandler_Base
{
    [Space(15)]
    [Header("=== Component Specific Fields ===")]
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
        else if(_tag.Contains(TagList.enemyTag) /*== TagList.enemyTag*/)
        {
            // Collision with enemy.
            Debug.Log("Parent: " + other.transform.parent.name);
            Debug.Log("Tag: " + _tag);
            Debug.Log(other.name);
            float damage = other.gameObject.GetComponentInParent<StateHandler_Base>().GetHitDamage();
            HandleDamage(damage);
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
        //stateProperties.totalHealth -= damage;
        totalHealth -= damage;

        UpdateHealthBar();
        StartCoroutine(EditMaterial(damageColor));

        //Debug.Log("Damage taken: " + totalHealth);
        //Edit material
        if (/*stateProperties.totalHealth*/totalHealth <= 0)
        {
            Debug.LogError("YOU DIED");
            if (dieEventSO != null)
                dieEventSO.RaiseEvent();
            
            //GameOver!
        }
    }

    protected override void HandleHealing(float health)
    {
        totalHealth += health;
        //stateProperties.totalHealth += health;

        UpdateHealthBar();
        StartCoroutine(EditMaterial(healingColor));

        if (totalHealth > maxHealth) totalHealth = maxHealth;
        //if (stateProperties.totalHealth > maxHealth) stateProperties.totalHealth = maxHealth;

        Debug.Log("Healing: " + totalHealth);
        //Debug.Log("Healing: " + stateProperties.totalHealth);
        
        //Edit material?
    }
}
