using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPooler))]
public class DestructorInstancer_Test : MonoBehaviour
{

    [SerializeField] int verticalResolution;
    [SerializeField] float horizontalSeparationPercentage = 0.25f;
    [SerializeField] float verticalOffset = 10;

    ObjectPooler objectPooler;

    void Awake()
    {
        objectPooler = GetComponent<ObjectPooler>();
    }

    [ContextMenu("Spawn From Pool")]
    public void SpawnFromPool()
    {
        GameObject[] objPack = objectPooler.SpawnPackFromPool(TagList.enemyDestructorT1Tag, 3);

        int centralPos = 0;
        Vector3 topScreen = GetTopScreenWorlPosition();

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
            enemy.transform.rotation = Quaternion.FromToRotation(enemy.transform.up, -enemy.transform.up);

            //Alternative to get rotation:
            //transform.localRotation * Quaternion.Euler(transform.forward * 180);
        }

    }

    private Vector3 GetTopScreenWorlPosition()
    {
        //new Vector3 
#if UNITY_ANDROID && !UNITY_EDITOR
        verticalResolution = Screen.height;
#endif

        return Camera.main.ScreenToWorldPoint(new Vector3(0, verticalResolution, 0));
    }
}
