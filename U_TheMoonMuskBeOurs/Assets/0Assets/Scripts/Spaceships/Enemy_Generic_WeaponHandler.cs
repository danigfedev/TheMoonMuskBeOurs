using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Generic_WeaponHandler : WeaponHandler_Base
{
    public override void OnTriggerEnter(Collider other)
    {
        //There are no weapon Power ups for the enemies,
        //therefore, there is no need to implement this method

        //throw new System.NotImplementedException();
    }
}
