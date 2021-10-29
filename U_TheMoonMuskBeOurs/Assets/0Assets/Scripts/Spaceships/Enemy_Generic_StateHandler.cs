using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Generic_StateHandler : StateHandler_Base
{
    protected override void OnTriggerEnter(Collider other)
    {
        //Control collisions with:
        //- Player bullet
        //- Player
        //- Shield
        if (other.tag == TagList.playerTag) 
        {
            float damage= other.gameObject.GetComponentInParent<StateHandler_Base>().GetHitDamage();
            HandleDamage(damage/*totalHealth*/);
        }
        //else if(other.tag == TagList.shieldTag)
        //{

        //}
        else if (other.tag == TagList.bulletPlayerTag)
        {
            //Debug.LogWarning(other.name);
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
