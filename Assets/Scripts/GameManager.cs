using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Level Settings")]
    public int level = 1;
    public int levelTime = 60;
    public int overalWawes;
    public bool isGameOver = false;
    public int gameDifficulty = 1;

    [Header("Difficulty Scaling")]
    private EnemyRespawner enemyRespawner;
    private int numberToIncreaseLvlDifficulty = 20;

    private float levelStartTime;
    private float remainingTime;
    private InGameUIManager uiManager;
    private PlayerController playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

    }

    void Start()
    {
        Debug.Log("started game manager");
        enemyRespawner = GameObject.Find("EnemyRespawner").GetComponent<EnemyRespawner>();
        uiManager = GameObject.Find("InGameUIManager").GetComponent<InGameUIManager>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        uiManager.SetLevelText("Level: " + level.ToString());
        levelStartTime = Time.time;  
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver) return;

        remainingTime = levelTime - (Time.time - levelStartTime );
        uiManager.SetTimeText("Time: " + Mathf.Clamp(Mathf.CeilToInt(remainingTime), 0, levelTime).ToString());

        if (remainingTime <= 0)
        {
            GetToNextLevel();
            Debug.Log("🏆 Level Complete! Now entering Level " + level);
        } else
        {
            ApplyDynamicDifficulty();
        }
        
        

    }
    public void ResetGame()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        isGameOver = false;
        uiManager.ToggleGsmeOverUi(false);
        SceneManager.LoadScene(currentSceneIndex);
    }
    public void GoToMain()
    {
        SceneManager.LoadScene(0);
    }
    // ye click bezarim bara raftan next level ya age game over bashe faqat reset
    public void gameOVer()
    {
    
        isGameOver = true;
        uiManager.ToggleGsmeOverUi(true);

    }
    void GetToNextLevel()
    {
        level++;
        UserSetting.Instance.cash = playerController.currentCash;
        playerController.ResetCash();
        if(UserSetting.Instance.highScoreLvl < level)
        {
           UserSetting.Instance.highScoreLvl = level;
        }
        DataPersistenceManager.SaveGame();
        uiManager.SetLevelText("Level: " + level.ToString());
        //inja mishe ye screen data bezarim o begim level tamam shod
        levelStartTime = Time.time;

        if (enemyRespawner != null)
        {
            return;
        }

    }
    void ApplyDynamicDifficulty()
    {
        // lelel progress is a number betwin 9 and 1 that present of start and end of the level
        float levelProgress = (Time.time - levelStartTime) / levelTime;

        // number to increase lvl difficulty is responible for the in each lvl from start to end how many times it should get harder the game (if it is 2 first half and second half)
        //level difficulty is only duty is get game harder in that lvl
        int lvlDifficulty = (int)Math.Floor(levelProgress * numberToIncreaseLvlDifficulty);
        // Ensure the difficulty doesn't go below the current level's base difficulty (level - 1)
        // You might want to adjust this based on your spawner logic
        if (enemyRespawner != null)
        {
            enemyRespawner.TimerBombIncreaseDiffcultyInLevel(lvlDifficulty);
        }
        
    }
    void ApplyDifficultyWithLevelPlanner()
    {
        
    }
}
