using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Van_StateHandler : StateHandler_Base
{

    protected override void OnTriggerEnter(Collider other)
    {
        //Control collisions with:
            //- Player bullet
            //- Player
            //- Shield
        if(other.tag==TagList.playerTag || other.tag == TagList.shieldTag)
            HandleDamage(totalHealth);
        else if(other.tag == TagList.bulletPlayerTag)
        {
            float damage = other.GetComponentInParent<Bullet>().GetBulletDamage();
            HandleDamage(damage);
        }
    }

    protected override void HandleDamage(float damage)
    {
        totalHealth -= damage;
        if (totalHealth <= 0)
        {
            //TODO Play Sound
            //TODO VFX
            gameObject.SetActive(false);
        }    
    }

    protected override void HandleHealing(float health)
    {
        throw new System.NotImplementedException("No healing for me");
    }

    

    
}
