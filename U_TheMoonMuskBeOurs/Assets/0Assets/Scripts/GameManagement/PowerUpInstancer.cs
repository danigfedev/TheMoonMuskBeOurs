using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPooler))]
public class PowerUpInstancer : MonoBehaviour
{
    [SerializeField] bool spawnOnStart = false;

    [SerializeField] float topMargin = 0f;
    [SerializeField] float sideMargin = 2f;

    private ObjectPooler backgroundPooler;
    private ScreenExtentsWorldSpace screenExtents;

    private void Awake()
    {
        backgroundPooler = GetComponent<ObjectPooler>();
        screenExtents = Utils.GetScreenWorldExtents();
    }

    private void Start()
    {
        if (spawnOnStart)
        {
            SpawnPowerUp(TagList.PU_healthTag);
            //Time.timeScale = 0;
        }
    }

    public void SpawnPowerUp(string powUp_Tag)
    {
        // Calculating spawn position

        //Adding +1 to include the last position in the range
        float posX = Random.Range((int)screenExtents.xMin+ sideMargin, (int)screenExtents.xMax - sideMargin + 1);
        float posY = screenExtents.yMax + topMargin;
        float posZ = 0;
        Vector3 position = new Vector3(posX, posY, posZ);

        backgroundPooler.SpawnSingleElementFromPool(powUp_Tag, position, Quaternion.identity);
    }
}
