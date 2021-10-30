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
    public static ScreenExtentsWorldSpace GetScreenWorldExtents()
    {
        Vector3 screenDimensions = new Vector3(Screen.width, Screen.height, 0);
        /*
#if UNITY_ANDROID && !UNITY_EDITOR
        screenDimensions = new Vector3(Screen.width, Screen.height, 0);
#else
        screenDimensions = new Vector3(screenWidth, screenHeight, 0);
#endif
        */

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
