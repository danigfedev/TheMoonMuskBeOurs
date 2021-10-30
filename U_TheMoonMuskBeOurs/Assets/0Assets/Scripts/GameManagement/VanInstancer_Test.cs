using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPooler))]
public class VanInstancer_Test : MonoBehaviour
{
    private struct ScreenExtentsWorldSpace
    {
        public float xMin, xMax, yMin, yMax, xMid, yMid, width, height;
    }


    [SerializeField] int screenHeight;
    [SerializeField] int screenWidth;
    [Range(0, 1)]
    [SerializeField] float verticalPercentageFromBottom = 0.75f;
    [SerializeField] float verticalOffsetLinearMotion = 2.5f;

    [Header("== TESTING ==")]
    [SerializeField] Vector3 rotationPivot; //For testing purposes
    [SerializeField] Vector3 spawnLateralPosition; //For testing purposes
    //[SerializeField] int screenHeight;
    //[SerializeField] int screenWidth;

    ObjectPooler objectPooler;

    void Awake()
    {
        objectPooler = GetComponent<ObjectPooler>();
    }

    private void Start()
    {
        SpawnFromPool();
    }

    [ContextMenu("Spawn From Pool")]
    public void SpawnFromPool()
    {
        GameObject[] objPack = objectPooler.SpawnPackFromPool(TagList.enemyVanT1Tag, 4);

        Vector2 extents = objPack[0].GetComponentInChildren<Collider>().bounds.extents;


        float Xoffset =  3 * extents.x;
        float Yoffset =  3 * extents.y;
        //float horDisplacement = 2 * extents.x + horoffset;

        //float verticalInitialPosition = topScreen.y + verticalOffset;
        //float vertDisplacement = verticalInitialPosition + 2 * extents.y;

        ScreenExtentsWorldSpace worldLimits = GetScreenWorldExtents();

        //Calculate pivot point:
        float pivotY = worldLimits.yMin + worldLimits.height * verticalPercentageFromBottom;
        rotationPivot = new Vector3(worldLimits.xMid, pivotY, 0);

        //Calculate spawn position:
        float verticalvalue = pivotY - verticalOffsetLinearMotion;

        float horizontalValue = worldLimits.xMax + Xoffset;
        //float horizontalValueNegative = -worldLimits.xMax - Xoffset;

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

    /// <summary>
    /// Returns screen's extents in world coordinates
    /// </summary>
    /// <returns></returns>
    private ScreenExtentsWorldSpace GetScreenWorldExtents()
    {
        Vector3 screenDimensions;
#if UNITY_ANDROID && !UNITY_EDITOR
        screenDimensions = new Vector3(Screen.width, Screen.height, 0);
#else
        screenDimensions = new Vector3(screenWidth, screenHeight, 0);
#endif

        Vector3 posMin = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 posMax = Camera.main.ScreenToWorldPoint(screenDimensions);

        ScreenExtentsWorldSpace extentsWorld;
        extentsWorld.xMin = posMin.x;
        extentsWorld.xMax = posMax.x;
        extentsWorld.yMin = posMin.y;
        extentsWorld.yMax = posMax.y;

        float width = extentsWorld.xMax - extentsWorld.xMin;
        float height = extentsWorld.yMax - extentsWorld.yMin;
        extentsWorld.width = width;
        extentsWorld.height = height;

        extentsWorld.xMid = posMin.x + width / 2;
        extentsWorld.yMid = posMin.y + height / 2;

        Debug.Log(string.Format("xMin: {0} | xMax: {1} | yMin: {2} | yMax: {3}",
            extentsWorld.xMin,
            extentsWorld.xMax,
            extentsWorld.yMin,
            extentsWorld.yMax));

        return extentsWorld;

        //return pos;
    }
}
