using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameStages
    {
        MENU = 0,
        STAGE_1,
        STAGE_2,
        STAGE_3
    }

    [Header("Intro")]
    [SerializeField] Transform cameraEnd;
    [SerializeField] Transform introEnvironment;
    [SerializeField] Vector3 envEndPos;

    [Space(5)]
    [Header("UI")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject gameMenu;
    [SerializeField] GameObject gameOverMenu;

    [Space(5)]
    [Header("Player")]
    [SerializeField] GameObject player;
    private PlayerController playerController;
    private Player_WeaponHandler playerWeaponHandler;

    [Header("Object Poolers")]
    [SerializeField] ObstacleInstancer obstacleInstancer;
    [SerializeField] VanInstancer vanInstancer;
    [SerializeField] DestructorInstancer destructorInstancer;
    [Space(10)]

    [Header("Stage data")]
    [SerializeField] GameStages currentState = GameStages.MENU;
    [Tooltip("Number of enemy waves to clear the current stage.")]
    [SerializeField] int stage_1_TotalWaves = 1;
    [SerializeField] int stage_1_EnemiesPerWave = 4;
    [Tooltip("Number of enemy waves to clear the current stage.")]
    [SerializeField] int stage_2_EnemiesPerWave = 3;
    [SerializeField] int stage_2_TotalWaves = 1;

    [Space(10)]
    [Header("Skybox data")]
    [SerializeField] Material skyboxMat;
    [SerializeField] 



    private int totalKills = 0;
    private int totalDestroyed = 0;
    private int currentWaveGoal;
    private int currentWaveEnemyCount;

    private void Awake()
    {
        playerController = player.GetComponent<PlayerController>();
        playerWeaponHandler = player.GetComponent<Player_WeaponHandler>();
    }

    void Start()
    {
        
    }

    //void Update()
    //{
        
    //}

    //Triggered form UI
    public void StartGame()
    {
        //Hide UI
        mainMenu.SetActive(false);

        //Hide Intro Scene
        StartCoroutine(HandleIntro());

        //Enable Player Control and shooting. Show fire

        //Lerp sky
        //Start Spawning things

    }

    public void OnEnemyDestroyedByGameLimits()
    {
        //Called by Object Pooler via SO event

        totalDestroyed++;
        if (!CheckWaveCompletion())
        {
            //SpawnNewWave
        }
        else
        {
            //UpdateStage
        }
    }

    public void OnEnemyKilled()
    {
        //Called by Enemy State Handler via SO event

        totalKills++;
        totalDestroyed++;
        //CheckWaveCompletion();
        if (!CheckWaveCompletion())
        {
            //SpawnNewWave
        }
        else
        {
            //UpdateStage
        }
        
    }

    private bool CheckWaveCompletion()
    {

        if(totalDestroyed % currentWaveGoal == 0
            && totalKills >= currentWaveGoal * currentWaveEnemyCount) //If wave destroyed
            return true;
        
        return false;
    }

    private void SpawnNewWave()
    {
        //Check current stage
        if (currentState == GameStages.STAGE_1)
        {
            vanInstancer.SpawnFromPool();
            //Drop Weapon 3, Health+ and Shield+ ???

        }
        else if (currentState == GameStages.STAGE_2)
        {
            destructorInstancer.SpawnFromPool();

            //Drop Weapon 3, Health+ and Shield+ ???

        }
        else
        {
            //Do stuff
        }
    }

    private void NextStage()
    {
        //Check current stage
        if (currentState == GameStages.STAGE_1)
        {
            //Launch sky lerp

            //vanInstancer -> Stop instancing objects
            //obstacleInstancer -> Change spawned obstacle

            //Drop Weapon 2, Health and Shield (Just to show that they are implemented)

            currentState = GameStages.STAGE_2;
        }
        else if (currentState == GameStages.STAGE_2)
        {
            //Launch sky lerp
            //Drop Weapon 3, Health+ and Shield+

        }
        else
        {

        }
    }


    private void GameOver()
    {
        SceneManager.LoadScene(0); //Just reload, for simplicity
    }


    private void EnablePlayerControl()
    {
        //playerController.EnableControl();
        playerWeaponHandler.Shoot(1);//.EnableShooting();/* = player.GetComponent<Player_WeaponHandler>();*/
    }


    float introElapsedTime = 0;
    float introTotalDuration = 2;
    private IEnumerator HandleIntro()
    {
        //Camera:
        Transform cam = Camera.main.transform;
        Vector3 initialPosition = cam.position;
        Quaternion initialRotation = cam.rotation;

        //Environment:
        Vector3 envInitialPos = introEnvironment.position;
        
        while (introElapsedTime <= 1)
        {
            //Update cam:
            cam.position = Vector3.Lerp(initialPosition, cameraEnd.position, introElapsedTime);
            cam.localRotation = Quaternion.Lerp(initialRotation, cameraEnd.localRotation, introElapsedTime);

            //Hide Intro environment:
            introEnvironment.position = Vector3.Lerp(envInitialPos, envEndPos, introElapsedTime);

            introElapsedTime += Time.deltaTime / introTotalDuration;
            yield return null;
        }

        //Disable Intro Environment:
        introEnvironment.gameObject.SetActive(false);

        EnablePlayerControl();
    }
    
    //Coroutine Lerp sky 

}
