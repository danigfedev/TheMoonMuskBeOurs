using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLimitHandler : MonoBehaviour
{
    public enum ScreenPositions
    {
        BOTTOM=0,
        TOP,
        LEFT,
        RIGHT
    }

    [SerializeField] ScreenPositions screenPosition;
    [SerializeField] GameObject biggestPoolableObjectPrefab;

    private Vector3 prefabExtents;

    void Awake()
    {
#if UNITY_ANDROID
        PlaceInScene();
#endif
    }

    [ContextMenu("Place In Scene")]
    public void PlaceInScene()
    {
        transform.localRotation = Quaternion.identity;
        transform.position = Vector3.zero;

        //limitExtents = GetComponent<Collider>().bounds.extents;
        prefabExtents = GetPrefabExtents();

        ScreenExtentsWorldSpace screenExtents = Utils.GetScreenWorldExtents();

        float offset = transform.localScale.y + 8 * prefabExtents.y;

        float posX = 0;
        float posY = 0;
        float rotZ = 0;
        if (screenPosition == ScreenPositions.TOP)
        {
            Debug.Log(ScreenPositions.TOP);
            posY = screenExtents.yMax + offset;
        }
            
        else if (screenPosition == ScreenPositions.BOTTOM)
            posY = screenExtents.yMin - offset;
        else if (screenPosition == ScreenPositions.LEFT)
        {
            rotZ = 90;
            posX = screenExtents.xMin - offset;
        }
        else if (screenPosition == ScreenPositions.RIGHT)
        {
            rotZ = 90;
            posX = screenExtents.xMax + offset;
        }

        transform.position = new Vector3(posX, posY, 0);
        transform.localRotation = Quaternion.Euler(0, 0, rotZ);

    }

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
