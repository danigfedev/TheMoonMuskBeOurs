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

    [Header("Stage 1 data")]
    [SerializeField] GameStages currentState = GameStages.MENU;
    [Tooltip("Number of enemy waves to clear the current stage.")]
    [SerializeField] int stage_1_TotalWaves = 1;
    [SerializeField] int stage_1_EnemiesPerWave = 4;
    
    [Space(5)]
    [Header("Stage 2 data")]
    [Tooltip("Number of enemy waves to clear the current stage.")]
    [SerializeField] int stage_2_TotalWaves = 1;
    [SerializeField] int stage_2_EnemiesPerWave = 3;

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







    #region === Event Driven Behaviour ===

    
    public void StartGame() //Triggered form UI
    {
        //Hide UI
        mainMenu.SetActive(false);

        //Hide Intro Scene
        StartCoroutine(HandleIntro());

        //Enable Player Control and shooting. Show fire

        //Lerp sky
        //Start Spawning things

    }

    public void OnPlayerDied()
    {
        Debug.LogError("[GameManager] Player Died");
        //TODO Enable Game over Screen
        
        
        //Testing purposes
        //RestartGame();
    }

    public void OnEnemyDestroyedByGameLimits()
    {
        

        totalDestroyed++;

        //Called by Object Pooler via SO event
        Debug.LogWarning("[GameManager] Enemy destroyed by game limits");
        Debug.Log(string.Format("Enemies Killed: {0} | Enemies destroyed: {1}", totalKills, totalDestroyed));
        //return;

        CheckWaveCompletion();
        /*
        if (!CheckWaveCompletion())
        {
            //SpawnNewWave();
        }
        else
        {
            //UpdateStage
        }
        */
    }

    public void OnEnemyKilled()
    {

        //Called by Enemy State Handler via SO event

        totalKills++;
        totalDestroyed++;

        Debug.LogWarning("[GameManager] Enemy killed");
        Debug.Log(string.Format("Enemies Killed: {0} | Enemies destroyed: {1}", totalKills, totalDestroyed));
        //return;

        CheckWaveCompletion();
        //if (totalDestroyed % currentWaveEnemyCount == 0)
        //{
        //    if(totalKills >= currentWaveGoal * currentWaveEnemyCount)
        //        NextStage();
        //    else
        //        SpawnNewWave();
        //}


        //If wave destroyed

        /*
        if (!CheckWaveCompletion())
        {
            SpawnNewWave();
        }
        else
            NextStage();
        
        */
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0); //Just reload, for simplicity
    }

    #endregion

    private void EnablePlayerControl()
    {
        playerController.EnableControl(true);
        playerWeaponHandler.Shoot(1);//.EnableShooting();/* = player.GetComponent<Player_WeaponHandler>();*/
    }

    //private void EnablePlayerShooting()
    //{

    //}

    private void/*bool*/ CheckWaveCompletion()
    {

        if (totalDestroyed % currentWaveEnemyCount == 0)
        {
            if (totalKills >= currentWaveGoal * currentWaveEnemyCount)
                NextStage();
            else
                SpawnNewWave();
        }


        /*
        Debug.Log("=== [CheckWaveCompletion]");
        Debug.Log(string.Format("Enemies Killed: {0} | Enemies destroyed: {1}", totalKills, totalDestroyed));
        if (totalDestroyed % currentWaveEnemyCount == 0
            && totalKills >= currentWaveGoal * currentWaveEnemyCount) //If wave destroyed
            return true;

        return false;
        */
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
        Debug.Log(string.Format("[GameManager] Stage {0} completed!", currentState));
        ResetEnemyWaveTotalCounts();
        //Debug.Log(string.Format("[GameManager] Moving to Next Stage from {0}", currentState));

        //Check current stage
        if (currentState== GameStages.MENU)
        {
            currentState = GameStages.STAGE_1;
            Debug.Log(string.Format("[GameManager] {0} reached!", currentState));

            //ResetEnemyWaveTotalCounts();
            currentWaveGoal = stage_1_TotalWaves;
            currentWaveEnemyCount = stage_1_EnemiesPerWave;

            //Launch sky lerp

            //Enemy and obstacle spawning:

            //obstacleInstancer -> Change spawned obstacle
            vanInstancer.SpawnFromPool();
            //Enable obstacle instancing
            
            //obstacleInstancer.Sta
        }
        else if (currentState == GameStages.STAGE_1)
        {
            currentState = GameStages.STAGE_2;
            Debug.Log(string.Format("[GameManager] {0} reached!", currentState));

            //ResetEnemyWaveTotalCounts();
            currentWaveGoal = stage_2_TotalWaves;
            currentWaveEnemyCount = stage_2_EnemiesPerWave;

            //Enemy and obstacle spawning:

            //obstacleInstancer -> Change spawned obstacle
            //vanInstancer -> Stop instancing objects, even destroy object pool recursively?
            destructorInstancer.SpawnFromPool();
            //Launch sky lerp

            

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

    private void ResetEnemyWaveTotalCounts()
    {
        totalKills = 0;
        totalDestroyed = 0;
    }

    float introElapsedTime = 0;
    float introTotalDuration = 2;
    private IEnumerator HandleIntro()
    {
        //Player Controller:
        playerController.StartEngines();

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

        NextStage();//Move from menu to Stage 1
        EnablePlayerControl();
    }
    
    //Coroutine Lerp sky 

}
