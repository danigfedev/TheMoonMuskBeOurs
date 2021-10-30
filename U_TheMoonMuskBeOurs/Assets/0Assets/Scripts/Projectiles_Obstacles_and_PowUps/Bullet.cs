using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    [SerializeField] protected float bulletDamage;
    [SerializeField] protected float bulletSpeed;
    
    protected Rigidbody bulletRB;

    //Definitions

    protected abstract void OnTriggerEnter(Collider other);

    
    //Implementations:

    protected void Awake() => bulletRB = GetComponent<Rigidbody>();
    protected virtual void Start() => Move();

    public float GetBulletDamage()
    {
        return bulletDamage;
    }

    //Override if necessary
    protected virtual void Move(/*float speed*/)
    {
        Vector3 bulletDirectionV3 = transform.up;
        bulletRB.velocity = bulletDirectionV3 * bulletSpeed;
    }

    /// <summary>
    /// Corutine that waits until the end of the current frame,
    /// thus making sure the rest of the objects complete their OnTriggerEnter behaviours
    /// </summary>
    /// <returns></returns>
    protected IEnumerator Destroy()
    {
        yield return new WaitForEndOfFrame();
        gameObject.SetActive(false);
    }
}
