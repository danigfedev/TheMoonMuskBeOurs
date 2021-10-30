using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLimitHandler : MonoBehaviour
{
    public enum ScreenPositions
    {
        BOTTOM=0,
        TOP
    }

    [SerializeField] int screenHeight;
    [SerializeField] int screenWidth;
    [SerializeField] ScreenPositions screenPosition;
    [SerializeField] GameObject biggestPoolableObjectPrefab;

    private Vector3 limitExtents;
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
        limitExtents = GetComponent<Collider>().bounds.extents;
        prefabExtents = GetPrefabExtents();
        Debug.Log(prefabExtents.y);
        

        Vector2 screenSize = GetScreenSize();
        //float verticalPos = (screenPosition == ScreenPositions.BOTTOM) ? 0 - offset : screenSize.y + offset;

        float verticalPos = 0;
        float offset = limitExtents.y + 8 * prefabExtents.y;
        if (screenPosition == ScreenPositions.TOP)
        
            verticalPos = GetScreenSize().y;
        else
            offset *= -1;

        transform.position = GetScreenWorlPosition(verticalPos, offset);
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

    private Vector2 GetScreenSize()
    {
        Vector2 screenSize;
#if UNITY_ANDROID && !UNITY_EDITOR
        screenSize.x = Screen.width;
        screenSize.y = Screen.height;
#elif UNITY_EDITOR
        screenSize.x = screenWidth;
        screenSize.y = screenHeight;
#endif
        return screenSize;
    }

    private Vector3 GetScreenWorlPosition(float verticalScreenPos, float offset)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        screenHeight = Screen.height;
#endif
        //offset = 0;
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(0, verticalScreenPos, 0));
        pos = new Vector3(0, pos.y += offset, 0);

        return pos;
    }
}
