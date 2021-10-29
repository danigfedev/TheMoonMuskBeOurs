using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPooler))]
public class Player_WeaponHandler : WeaponHandler_Base
{

    #region === MonoBehaviour Methods ===

    
    public override void OnTriggerEnter(Collider other)
    {
        //Debug.Log("This: " + gameObject.name + " | Other: " + other.name);
        switch (other.tag)
        {
            //Powerups
            case TagList.PU_weaponTag:
                int bulletCount = other.gameObject.GetComponent<PowerUpHandler>().GetPowerUpAmount();
                bulletCount = Mathf.Clamp(bulletCount, 1, 3);
                Shoot(bulletCount);
                break;
            //case TagList.PU_weaponPlayerGun1Tag:
            //    Shoot(1);
            //    break;
            //case TagList.PU_weaponPlayerGun2Tag:
            //    Shoot(2);
            //    break;
            //case TagList.PU_weaponPlayerGun3Tag:
            //    Shoot(3);
            //    break;
            case TagList.PU_weaponPlayerFlamethrowTag:
                Debug.LogError("Musk flamethrower not on sale yet.");
                break;
        }
        //other.gameObject.SetActive(false);
    }

    #endregion

}
