using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateHandler_Base: MonoBehaviour
{
    //[SerializeField] protected StateHandlerProperties stateProperties;
    [SerializeField] protected float totalHealth;
    [SerializeField] protected float hitDamage;
    protected float maxHealth;
    
    // Definitions:

    protected abstract void OnTriggerEnter(Collider other);
    protected abstract void HandleDamage(float damage);
    protected abstract void HandleHealing(float health);


    //Implementations:

    public virtual void Awake()
    {
        maxHealth = totalHealth;
        //maxHealth = stateProperties.totalHealth;
    }

    public float GetHitDamage() => hitDamage;/*stateProperties.hitDamage;*/
}
