using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPooler))]
public class WeaponHandler : MonoBehaviour
{
    public enum BulletDirections
    {
        VERTICALLY_ALIGNED = 0, //Following shooting object's vertical axis
        VERTICALLY_OPPOSED //opposing shooting object's vertical axis
    }

    public enum BuletTypes
    {
        PLAYER=0,
        BOX,
        SMILE,
        OTHER
    }

//#if UNITY_EDITOR
    [Header("== TESTING ==")]
    public bool allowShooting = true;
    [Range(1, 3)] public int bulletCount = 1;
    [Space(5)]
//#endif

    [SerializeField] Transform bulletSpawnPosition;

    [SerializeField] BulletDirections bulletDirection;
    [SerializeField] BuletTypes bulletType;
    
    private ObjectPooler bulletPooler;
    private Coroutine shootingCoroutine = null;
    private string bulletTag;

    #region === MonoBehaviour Methods ===
    private void Awake()
    {
        bulletPooler = GetComponent<ObjectPooler>();
    }

    private void Start()
    {
        if (allowShooting)
            Shoot(bulletCount, bulletDirection);

        switch (bulletType)
        {
            case BuletTypes.PLAYER:
                bulletTag = TagList.bulletPlayerTag;
                break;
            case BuletTypes.BOX:
                bulletTag = TagList.bulletBoxTag;
            break;
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        // Collisions filtered based on collision matrix
        string otherTag = other.tag;
        if(otherTag == TagList.bulletPlayerTag
            || otherTag== TagList.shieldTag) return;

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
    public void Shoot(int bulletCount, BulletDirections direction= BulletDirections.VERTICALLY_ALIGNED)
    {
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }


        shootingCoroutine = StartCoroutine(ShootCoroutine(bulletCount, direction));
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
        bulletPooler.ResetPool(TagList.bulletPlayerTag);
    }

    float elapsedTime = 0;
    [Tooltip("Firerate measured in bullets/second")]
    public float fireRate = 1;
    private IEnumerator ShootCoroutine(int bulletCount, BulletDirections direction)
    {
        while (true)
        {
            if (elapsedTime >= 1)
            {
                Quaternion rot = (direction == BulletDirections.VERTICALLY_ALIGNED) ?
                    transform.localRotation
                    : Quaternion.FromToRotation(transform.up, -transform.up);
                
                //Alternative to get rotation:
                //transform.localRotation * Quaternion.Euler(transform.forward * 180);
                
                bulletPooler.SpawnPackFromPool(bulletTag, bulletCount, bulletSpawnPosition.position, rot, 0.1f);


                //TODO Play Shot Sound

                elapsedTime = 0;
            }
            elapsedTime += Time.deltaTime * fireRate;
            yield return null;
        }
    }

    /// <summary>
    /// Given a number of items and a base position, linearly distributes the items along the X axis
    /// and around the given position based on their extents and an offset value to add some separation.
    /// </summary>
    /// <param name="basePos"></param>
    /// <param name="packSize"></param>
    /// <param name="horExtents"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    private Vector3[] CalculatePackPositions(Vector3 basePos, int packSize, float horExtents, float offset)
    {
        if (packSize <= 0)
        {
            Debug.LogError("Invalid pack size. Plece introduce a value higher than 0");
            return null;
        }

        int initialIndex = 0;
        Vector3[] positions = new Vector3[packSize];

        if (packSize % 2 != 0)
        {
            initialIndex = 1;
            positions[0] = basePos;
        }


        for (int i = initialIndex; i <= packSize - 2; i += 2)
        {
            float horDisplacement = (i + 1) * (horExtents + offset / 2);
            Vector3 displacement = Vector3.right * horDisplacement;
            positions[i] = basePos + displacement;
            positions[i + 1] = basePos - displacement;
        }

        return positions;
    }


}
