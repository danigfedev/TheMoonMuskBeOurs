using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct ScreenExtentsWorldSpace
{
    public float xMin, xMax, yMin, yMax, xMid, yMid, width, height;

}

public class Utils
{

    /// <summary>
    /// Returns screen's extents in world coordinates
    /// </summary>
    /// <returns></returns>
    public static ScreenExtentsWorldSpace GetScreenWorldExtents(bool debugValues=false)
    {
        /*
        Vector3 screenDimensions;
#if UNITY_EDITOR
        screenDimensions = new Vector3(1080, 2340, 0); //Hardcoded to avoid problems in editor
#elif UNITY_ANDROID
        screenDimensions = new Vector3(Screen.width, Screen.height, 0);
#endif
        */
        Vector3 screenDimensions = new Vector3(Screen.width, Screen.height, 0);

        Transform utilsCam = GameObject.FindGameObjectWithTag(TagList.utilsCamTag).transform.GetChild(0);
        utilsCam.gameObject.SetActive(true);
        Camera cam = utilsCam.GetComponent<Camera>();

        Vector3 posMin = cam.ScreenToWorldPoint(Vector3.zero);
        Vector3 posMax = cam.ScreenToWorldPoint(screenDimensions);

        utilsCam.gameObject.SetActive(false);


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

        if (debugValues)
        {
            Debug.Log(string.Format("Screen dimensions: {0}", screenDimensions));
            Debug.Log(string.Format("xMin: {0} | xMax: {1} | yMin: {2} | yMax: {3}",
                extentsWorld.xMin,
                extentsWorld.xMax,
                extentsWorld.yMin,
                extentsWorld.yMax));
        }
       

        return extentsWorld;

        //return pos;
    }
}
