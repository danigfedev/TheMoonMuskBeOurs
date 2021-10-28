using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPooler))]
public class WeaponHandler : MonoBehaviour
{

#if UNITY_EDITOR
    [Header("== TESTING ==")]
    public bool allowShooting = true;
    [Range(1, 3)] public int bulletCount = 1;
    [Space(5)]
#endif

    [SerializeField] private Transform bulletSpawnPosition;

    private ObjectPooler bulletPooler;
    private Coroutine shootingCoroutine = null;

    #region === MonoBehaviour Methods ===
    private void Awake()
    {
        bulletPooler = GetComponent<ObjectPooler>();
    }

    private void Start()
    {
        if (allowShooting)
            Shoot(bulletCount);
    }

    
    private void OnTriggerEnter(Collider other)
    {
        // Collisions filtered based on collision matrix
        
        if (other.tag == TagList.bulletTag) return;

        switch (other.tag)
        {
            case TagList.PU_weaponPlayerGun1Tag:
                Shoot(1);
                break;
            case TagList.PU_weaponPlayerGun2Tag:
                Shoot(2);
                break;
            case TagList.PU_weaponPlayerGun3Tag:
                Shoot(3);
                break;
            case TagList.PU_weaponPlayerFlamethrowTag:
                Debug.LogError("Musk flamethrower not on sale yet.");
                break;
            //default:
            //    Shoot(1);
            //    break;
        }

        other.gameObject.SetActive(false);
    }
    

    #endregion

    #region === Editor Only Code ===

#if UNITY_EDITOR

    [ContextMenu("Shoot One")]
    public void ShootOne()
    {
        Shoot(1);
    }
    [ContextMenu("Shoot Two")]
    public void ShootTwo()
    {
        Shoot(2);
    }
    [ContextMenu("Shoot Three")]
    public void ShootThree()
    {
        Shoot(3);
    }
    [ContextMenu("Stop Shooting")]
    public void DisableShooting()
    {
        StopShooting();
    }

#endif

    #endregion

    /// <summary>
    /// Starts or updates the shooting process
    /// </summary>
    /// <param name="bulletCount">The number of bullets that should be spawned per shot</param>
    public void Shoot(int bulletCount)
    {
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }

        shootingCoroutine = StartCoroutine(ShootCoroutine(bulletCount));
    }

    /// <summary>
    /// Stops the spaceship from shooting
    /// </summary>
    private void StopShooting()
    {
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
        elapsedTime = 0;
        bulletPooler.ResetPool(TagList.bulletTag);
    }

    float elapsedTime = 0;
    [Tooltip("Firerate measured in bullets/second")]
    public float fireRate = 1;
    private IEnumerator ShootCoroutine(int bulletCount)
    {
        while (true)
        {
            if (elapsedTime /*/ fireRate*/ >= 1)
            {
                //bulletPooler.SpawnSingleElementFromPool(TagList.bulletTag, bulletSpawnPosition.position);
                bulletPooler.SpawnPackFromPool(TagList.bulletTag, bulletCount, bulletSpawnPosition.position, 0.1f);
                
                //TODO Play Shot Sound

                elapsedTime = 0;
            }
            elapsedTime += Time.deltaTime * fireRate;
            yield return null;
        }
    }
}
