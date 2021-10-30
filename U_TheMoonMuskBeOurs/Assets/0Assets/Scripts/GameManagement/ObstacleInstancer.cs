using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleInstancer : MonoBehaviour
{
    [SerializeField] bool isBackground = false;
    [Range(0, 1)]
    [SerializeField] float satelliteObstacleScreenPctg = 0.75f;
    [SerializeField] float instanceRate = 0.1f;
    [SerializeField] float topMargin = 4f;
    [SerializeField] float sideMargin = 4f;
    [SerializeField] float backgrounDepthPosition = 8;

    private ObjectPooler backgroundPooler;
    //protected Coroutine shootingCoroutine = null;
    //protected string bulletTag;
    private float shootingElapsedTime = 0;
    private ScreenExtentsWorldSpace screenExtents;

    private void Awake()
    {
        backgroundPooler = GetComponent<ObjectPooler>();
        screenExtents = Utils.GetScreenWorldExtents();
    }

    private void Start()
    {
        //StartCoroutine(SpawnCloud());
        StartCoroutine(SpawnSatellite());
    }

    private IEnumerator SpawnCloud()
    {
        while (true)
        {
            if (shootingElapsedTime >= 1)
            {
                //Quaternion rot = (direction == BulletDirections.VERTICALLY_ALIGNED) ?
                //    transform.localRotation
                //    : Quaternion.FromToRotation(transform.up, -transform.up);

                //Alternative to get rotation:
                //transform.localRotation * Quaternion.Euler(transform.forward * 180);
                float likelihood = Random.Range(0, 2);
                string _tag;
                if(isBackground)
                   _tag = (likelihood >= 0.5f) ? TagList.cloudBG1_Tag : TagList.cloudBG2_Tag;
                else
                    _tag = (likelihood >= 0.5f) ? TagList.cloudFG1_Tag : TagList.cloudFG2_Tag;

                // Calculating spawn position
                float posX = Random.Range((int)screenExtents.xMin, (int)screenExtents.xMax + 1);//Adding +1 to include the last position in the range
                float posY = screenExtents.yMax + topMargin;
                Vector3 position = new Vector3(posX, posY, backgrounDepthPosition);

                backgroundPooler.SpawnSingleElementFromPool(_tag, position, Quaternion.identity);

                //TODO Play Shot Sound

                shootingElapsedTime = 0;
            }
            shootingElapsedTime += Time.deltaTime * instanceRate;
            yield return null;
        }
    }


    
    private IEnumerator SpawnSatellite()
    {
        //TODO Create Game Limits at both sides

        while (true)
        {
            if (shootingElapsedTime >= 1)
            {
                float likelihood = Random.Range(0, 2);
                float posX = (likelihood >= 0.5f) ? screenExtents.xMax + sideMargin : screenExtents.xMin - sideMargin;

                // Calculate spawn position
                //float satelliteScreenPctg = 1;
                float pctgFactor = (isBackground) ? 0 : 1 - satelliteObstacleScreenPctg;
                float freeHeight = screenExtents.height * pctgFactor;// (1 - satelliteScreenPctg);
                float posY = Random.Range(screenExtents.yMin + freeHeight / 2, screenExtents.yMax - freeHeight / 2);
                float posZ = isBackground ? backgrounDepthPosition : 0;
                Vector3 position = new Vector3(posX, posY, posZ);

                string _tag = isBackground ? TagList.satelliteBG_Tag : TagList.satelliteFG_Tag;
                GameObject obstacle = backgroundPooler.SpawnSingleElementFromPool(_tag, position, Quaternion.identity);

                // Adjusting speed and rotation:
                float rotZ = 30;
                if (likelihood >= 0.5f)
                {
                    rotZ = -30;
                    obstacle.GetComponent<Bullet_Sinusoidal>().UseNegativeSpeed();
                }
                obstacle.transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, rotZ);
                

               //TODO Play Shot Sound

                shootingElapsedTime = 0;
            }
            shootingElapsedTime += Time.deltaTime * instanceRate;
            yield return null;
        }

    }
    



}
