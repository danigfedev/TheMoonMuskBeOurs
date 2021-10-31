using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPooler))]
public class DestructorInstancer : MonoBehaviour
{
    [SerializeField] bool spawnOnStart = false;
    //[SerializeField] int verticalResolution;
    [SerializeField] float horizontalSeparationPercentage = 0.25f;
    [SerializeField] GameObject biggestPoolableObjectPrefab;
    [SerializeField] int prefabCountAsMargin = 4;

    float verticalOffset = 0;
    ObjectPooler objectPooler;

    void Awake()
    {
        objectPooler = GetComponent<ObjectPooler>();
    }

    private void Start()
    {
        verticalOffset = prefabCountAsMargin * GetPrefabExtents().y;

        if(spawnOnStart)
            SpawnFromPool();
    }

    [ContextMenu("Spawn From Pool")]
    public void SpawnFromPool()
    {
        GameObject[] objPack = objectPooler.SpawnPackFromPool(TagList.enemyDestructorT1Tag, 3);

        int centralPos = 0;
        ScreenExtentsWorldSpace screenExtents = Utils.GetScreenWorldExtents();
        Vector3 topScreen = new Vector3(screenExtents.xMid, screenExtents.yMax, 0);

        Vector2 extents = objPack[0].GetComponentInChildren<Collider>().bounds.extents;
        float horoffset = horizontalSeparationPercentage * extents.x;
        float horDisplacement = 2 * extents.x + horoffset;

        float verticalInitialPosition = topScreen.y + verticalOffset;
        float vertDisplacement = verticalInitialPosition + 2 * extents.y;

        objPack[0].transform.position = new Vector3(centralPos, verticalInitialPosition);
        objPack[1].transform.position = new Vector3(centralPos + horDisplacement, vertDisplacement);
        objPack[2].transform.position = new Vector3(centralPos - horDisplacement, vertDisplacement);
        
        
        foreach(GameObject enemy in objPack)
        {
            enemy.transform.localRotation = transform.localRotation * Quaternion.Euler(transform.forward * 180);
                
                //Quaternion.FromToRotation(enemy.transform.up, -enemy.transform.up);

            //Alternative to get rotation:
            //transform.localRotation * Quaternion.Euler(transform.forward * 180);
        }

    }

    /*
    private Vector3 GetTopScreenWorlPosition()
    {
        //new Vector3 
#if UNITY_ANDROID && !UNITY_EDITOR
        verticalResolution = Screen.height;
#endif

        return Camera.main.ScreenToWorldPoint(new Vector3(0, verticalResolution, 0));
    }
    */

    private Vector3 GetPrefabExtents()
    {
        GameObject instance = Instantiate(biggestPoolableObjectPrefab,
            Vector3.forward * 100,
            Quaternion.identity);
        Vector3 e = instance.GetComponentInChildren<Collider>().bounds.extents;
        DestroyImmediate(instance);//Only done on load, not a problem in runtime
        return e;
    }
}
