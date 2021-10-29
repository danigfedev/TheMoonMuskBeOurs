using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateHandler
{
    void OnTriggerEnter(Collider other);
    void HandleDamage(float damage);
    void HandleHealing(float health);
    
}
