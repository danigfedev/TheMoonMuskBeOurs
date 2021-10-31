using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPooler))]
public class Player_WeaponHandler : WeaponHandler_Base
{

    #region === MonoBehaviour Methods ===

    
    public override void OnTriggerEnter(Collider other)
    {
        string _tag = other.tag;
        if (_tag.Contains(TagList.PU_weaponTag))
        {
            int bulletCount = other.gameObject.GetComponent<PowerUpHandler>().GetPowerUpAmount();
            bulletCount = Mathf.Clamp(bulletCount, 1, 3);
            Shoot(bulletCount);
        }

        //other.gameObject.SetActive(false);
    }

    #endregion

}
