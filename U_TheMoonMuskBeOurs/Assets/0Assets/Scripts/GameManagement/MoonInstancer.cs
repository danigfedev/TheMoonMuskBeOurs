using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPooler))]

public class MoonInstancer : MonoBehaviour
{

    [SerializeField] bool spawnOnStart = false;
    [Space(10)]
    [SerializeField] bool isBackground = true;
    [SerializeField] int topMargin = 5;
    [Range(0, 1)]
    [SerializeField] float endPositionPctg = 0.75f;
    [SerializeField] float backgrounDepthPosition = 8;



    private ObjectPooler backgroundPooler;
    private ScreenExtentsWorldSpace screenExtents;

    private void Awake()
    {
        backgroundPooler = GetComponent<ObjectPooler>();
        screenExtents = Utils.GetScreenWorldExtents(true);
    }

    private void Start()
    {
        if (spawnOnStart) SpawnMoon();

    }

    public void SpawnMoon()
    {
        //float likelihood = Random.Range(0, 2);
        //string _tag;
        //if (isBackground)
        //    _tag = (likelihood >= 0.5f) ? TagList.cloudBG1_Tag : TagList.cloudBG2_Tag;
        //else
        //    _tag = (likelihood >= 0.5f) ? TagList.cloudFG1_Tag : TagList.cloudFG2_Tag;

        // Calculating spawn position
        //float posX = 0;
        float posZ = isBackground ? backgrounDepthPosition : 0;
        Vector3 startPosition = new Vector3(screenExtents.xMid, screenExtents.yMax + topMargin, posZ);
        float posY = screenExtents.yMin + endPositionPctg * screenExtents.height;
        Vector3 endPposition = new Vector3(screenExtents.xMid, posY, posZ);
        //Debug.LogWarning(endPposition);
        GameObject moonObj = backgroundPooler.SpawnSingleElementFromPool(TagList.moonTag, startPosition, Quaternion.identity);

        moonObj.transform.position = startPosition;
        moonObj.GetComponent<MoonController>().SetMotionParameters(startPosition, endPposition);
    }
}
