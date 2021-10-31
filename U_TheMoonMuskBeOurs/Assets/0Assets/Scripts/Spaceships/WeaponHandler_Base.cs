using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponHandler_Base: MonoBehaviour
{
    public enum BulletDirections
    {
        VERTICALLY_ALIGNED = 0, //Following shooting object's vertical axis
        VERTICALLY_OPPOSED //opposing shooting object's vertical axis
    }

    public enum BulletTypes
    {
        PLAYER = 0,
        ENEMY,
        BOX,
        SMILE,
        OTHER
    }

    [Header("== TESTING ==")]
    public bool allowShooting = true;
    [Range(1, 3)] public int bulletCount = 1;
    [Space(10)]
    //Fields:
    [SerializeField] protected BulletTypes bulletType;
    [SerializeField] protected BulletDirections bulletDirection;
    [SerializeField] protected Transform bulletSpawnPosition;

    [Tooltip("Firerate measured in bullets/second")]
    [SerializeField] protected float fireRate = 1;

    protected ObjectPooler bulletPooler;
    protected Coroutine shootingCoroutine = null;
    protected string bulletTag;
    protected float shootingElapsedTime = 0;

    //Methods:
    public abstract void OnTriggerEnter(Collider other);

    protected void Awake()
    {
        bulletPooler = GetComponent<ObjectPooler>();
    }

    public virtual void Start()
    {
        switch (bulletType)
        {
            case BulletTypes.PLAYER:
                bulletTag = TagList.bulletPlayerTag;
                break;
            case BulletTypes.ENEMY:
                bulletTag = TagList.bulletEnemyGenericTag;
                break;
            case BulletTypes.BOX:
                bulletTag = TagList.bulletBoxTag;
                break;
            case BulletTypes.SMILE:
                bulletTag = TagList.bulletSmileTag;
                break;
            default:
                Debug.LogError("[WeaponHandler_Base] Error getting bullet tag");
                break;
        }

        if (allowShooting)
            Shoot(bulletCount, bulletDirection);
    }

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
    public void Shoot(int bulletCount, BulletDirections direction = BulletDirections.VERTICALLY_ALIGNED)
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
    protected void StopShooting()
    {
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
        shootingElapsedTime = 0;
        bulletPooler.ResetPool(TagList.bulletPlayerTag);
    }

    protected IEnumerator ShootCoroutine(int bulletCount, BulletDirections direction)
    {
        shootingElapsedTime = Random.Range((float)0, (float)1);

        while (true)
        {
            if (shootingElapsedTime >= 1)
            {
                Quaternion rot = (direction == BulletDirections.VERTICALLY_ALIGNED) ?
                    transform.localRotation
                    : Quaternion.FromToRotation(transform.up, -transform.up);

                //Alternative to get rotation:
                //transform.localRotation * Quaternion.Euler(transform.forward * 180);

                bulletPooler.SpawnPackFromPool(bulletTag, bulletCount, bulletSpawnPosition.position, rot, 0.1f);


                //TODO Play Shot Sound

                shootingElapsedTime = 0;
            }
            shootingElapsedTime += Time.deltaTime * fireRate;
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
