using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPooler))]
public class VanInstancer : MonoBehaviour
{
    [SerializeField] bool spawnOnStart = false;
    [Range(0, 1)]
    [SerializeField] float verticalPercentageFromBottom = 0.75f;
    [SerializeField] float verticalOffsetLinearMotion = 2.5f;

    Vector3 rotationPivot;
    ObjectPooler objectPooler;

    void Awake()
    {
        objectPooler = GetComponent<ObjectPooler>();
    }

    private void Start()
    {
        if(spawnOnStart)
            SpawnFromPool();
    }

    [ContextMenu("Spawn From Pool")]
    public void SpawnFromPool()
    {
        Debug.LogWarning("[Van] Spawning from pool");
        GameObject[] objPack = objectPooler.SpawnPackFromPool(TagList.enemyVanT1Tag, 4);

        Vector2 extents = objPack[0].GetComponentInChildren<Collider>().bounds.extents;

        float Xoffset =  3 * extents.x;
        float Yoffset =  3 * extents.y;

        ScreenExtentsWorldSpace worldLimits = Utils.GetScreenWorldExtents();

        //Calculate pivot point:
        float pivotY = worldLimits.yMin + worldLimits.height * verticalPercentageFromBottom;
        rotationPivot = new Vector3(worldLimits.xMid, pivotY, 0);

        //Calculate spawn position:
        float verticalvalue = pivotY - verticalOffsetLinearMotion;

        float horizontalValue = worldLimits.xMax + Xoffset;

        //Screen Horizontal limit + offset


        //Spawn Layout:
        //--------------------------------- Pivot_Point -----------------------------------
        //-------------------------- verticalOffsetLinearMotion ---------------------------
        //Obj[0]----|Screen Left Margin ------ xMid ------ Screen Right Margin|----Obj[2]
        //----------------------------------- Yoffset -----------------------------------
        //Obj[1]----|Screen Left Margin ------ xMid ------ Screen Right Margin|----Obj[3]

        Vector3 endPosition;

        objPack[0].transform.position = new Vector3(-horizontalValue, verticalvalue);
        endPosition= new Vector3(rotationPivot.x - Xoffset, rotationPivot.y + Xoffset);
        objPack[0].GetComponent<Enemy_Van_Controller>().SetMotionParameters(objPack[0].transform.position, endPosition, rotationPivot);

        objPack[1].transform.position = new Vector3(-horizontalValue, verticalvalue - Yoffset);
        endPosition = new Vector3(rotationPivot.x - Xoffset, rotationPivot.y - Xoffset);
        objPack[1].GetComponent<Enemy_Van_Controller>().SetMotionParameters(objPack[1].transform.position, endPosition, rotationPivot);

        objPack[2].transform.position = new Vector3(horizontalValue, verticalvalue);
        endPosition = new Vector3(rotationPivot.x + Xoffset, rotationPivot.y + Xoffset);
        objPack[2].GetComponent<Enemy_Van_Controller>().SetMotionParameters(objPack[2].transform.position, endPosition, rotationPivot);

        objPack[3].transform.position = new Vector3(horizontalValue, verticalvalue - Yoffset);
        endPosition = new Vector3(rotationPivot.x + Xoffset, rotationPivot.y - Xoffset);
        objPack[3].GetComponent<Enemy_Van_Controller>().SetMotionParameters(objPack[3].transform.position, endPosition, rotationPivot);

    }

}
