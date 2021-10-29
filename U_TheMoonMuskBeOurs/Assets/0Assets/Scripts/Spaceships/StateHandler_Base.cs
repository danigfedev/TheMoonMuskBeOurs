using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateHandler_Base: MonoBehaviour
{

    [SerializeField] protected float totalHealth;
    protected float maxHealth;

    protected abstract void OnTriggerEnter(Collider other);
    protected abstract void HandleDamage(float damage);
    protected abstract void HandleHealing(float health);


    //Implementations:
    public virtual void Awake()
    {
        maxHealth = totalHealth;
    }
}
