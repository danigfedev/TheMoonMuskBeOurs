using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPooler))]
public class VanInstancer_Test : MonoBehaviour
{
    [SerializeField] Vector3 rotationPivot; //For testing purposes
    [SerializeField] Vector3 spawnLateralPosition; //For testing purposes
    //[SerializeField] int screenHeight;
    //[SerializeField] int screenWidth;

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

        //Camera.main.ScreenToWorldPoint()

        Vector3 endPosition;
        objPack[0].transform.position = new Vector3(-spawnLateralPosition.x, spawnLateralPosition.y);
        endPosition= new Vector3(rotationPivot.x - offset, rotationPivot.y + offset);
        objPack[0].GetComponent<Enemy_Van_Controller>().SetMotionParameters(objPack[0].transform.position, endPosition, rotationPivot);

        objPack[1].transform.position = new Vector3(-spawnLateralPosition.x, spawnLateralPosition.y - 3 * extents.y);
        endPosition = new Vector3(rotationPivot.x - offset, rotationPivot.y - offset);
        objPack[1].GetComponent<Enemy_Van_Controller>().SetMotionParameters(objPack[1].transform.position, endPosition, rotationPivot);

        objPack[2].transform.position = new Vector3(spawnLateralPosition.x, spawnLateralPosition.y);
        endPosition = new Vector3(rotationPivot.x + offset, rotationPivot.y + offset);
        objPack[2].GetComponent<Enemy_Van_Controller>().SetMotionParameters(objPack[2].transform.position, endPosition, rotationPivot);

        objPack[3].transform.position = new Vector3(spawnLateralPosition.x, spawnLateralPosition.y - 3 * extents.y);
        endPosition = new Vector3(rotationPivot.x + offset, rotationPivot.y - offset);
        objPack[3].GetComponent<Enemy_Van_Controller>().SetMotionParameters(objPack[3].transform.position, endPosition, rotationPivot);



        /*
        objPack[0].transform.position = new Vector3(rotationPivot.x - offset, rotationPivot.y - offset);
        objPack[1].transform.position = new Vector3(rotationPivot.x - offset, rotationPivot.y + offset);
        objPack[2].transform.position = new Vector3(rotationPivot.x + offset, rotationPivot.y - offset);
        objPack[3].transform.position = new Vector3(rotationPivot.x + offset, rotationPivot.y + offset);

        foreach (GameObject enemy in objPack)
        {
            //enemy.GetComponent<Enemy_Van_Controller>().SetPivotPoint(spawnPosition);
            enemy.GetComponent<Enemy_Van_Controller>().StartMoving(,rotationPivot);
        }
        */
    }
}
