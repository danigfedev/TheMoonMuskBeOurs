using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPooler))]
public class VanInstancer_Test : MonoBehaviour
{
    [SerializeField] Vector3 spawnPosition; //For testing purposes

    ObjectPooler objectPooler;

    void Awake()
    {
        objectPooler = GetComponent<ObjectPooler>();
    }

    [ContextMenu("Spawn From Pool")]
    public void SpawnFromPool()
    {
        GameObject[] objPack = objectPooler.SpawnPackFromPool(TagList.enemyVanT1Tag, 4);

        Vector2 extents = objPack[0].GetComponentInChildren<Collider>().bounds.extents;


        float offset =  3 * extents.x;
        //float horDisplacement = 2 * extents.x + horoffset;

        //float verticalInitialPosition = topScreen.y + verticalOffset;
        //float vertDisplacement = verticalInitialPosition + 2 * extents.y;

        objPack[0].transform.position = new Vector3(spawnPosition.x - offset, spawnPosition.y - offset);
        objPack[1].transform.position = new Vector3(spawnPosition.x - offset, spawnPosition.y + offset);
        objPack[2].transform.position = new Vector3(spawnPosition.x + offset, spawnPosition.y - offset);
        objPack[3].transform.position = new Vector3(spawnPosition.x + offset, spawnPosition.y + offset);

        foreach (GameObject enemy in objPack)
        {
            enemy.GetComponent<Enemy_Van_Controller>().SetPivotPoint(spawnPosition);
        }

    }
}
