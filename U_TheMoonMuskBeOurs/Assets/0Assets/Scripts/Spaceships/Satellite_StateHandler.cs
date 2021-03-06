using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite_StateHandler : StateHandler_Base
{
    protected override void OnTriggerEnter(Collider other)
    {
        //Control collisions with:
        //- Player bullet
        //- Player
        //- Shield
        if (other.tag == TagList.playerTag)
        {
            float damage = other.gameObject.GetComponentInParent<StateHandler_Base>().GetHitDamage();
            HandleDamage(damage);
        }
        else if (other.tag == TagList.shieldTag)
        {
            StartCoroutine(Destroy());
        }
        else if (other.tag == TagList.bulletPlayerTag)
        {
            float damage = other.GetComponentInParent<Bullet>().GetBulletDamage();
            HandleDamage(damage);
        }
    }

    protected override void HandleDamage(float damage)
    {
        StartCoroutine(EditMaterial(damageColor));
        //UpdateHealthBar();

        totalHealth -= damage;
        if (totalHealth <= 0)
        {
            //TODO Play Sound
            //TODO VFX

            if (dieEventSO != null) dieEventSO.RaiseEvent();
            StartCoroutine(Destroy());
        }
    }

    protected override void HandleHealing(float health)
    {
        throw new System.NotImplementedException("No healing for me");
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForEndOfFrame();
        gameObject.SetActive(false);
    }
}
