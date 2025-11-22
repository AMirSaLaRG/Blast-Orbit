using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Level Settings")]
    public int level = 1;
    public int levelTime = 60;
    public int overalWawes;

    [Header("Difficulty Scaling")]
    private EnemyRespawner enemyRespawner;
    private int numberToIncreaseLvlDifficulty = 3;

    private float levelStartTime;
    private float remainingTime;
    public bool isGameOver = false;
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
    // ye click bezarim bara raftan next level ya age game over bashe faqat reset
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
        float levelProgress = (Time.time - levelStartTime) / levelTime;

        int lvlDifficulty = (int)Math.Floor(levelProgress * numberToIncreaseLvlDifficulty);
        // Ensure the difficulty doesn't go below the current level's base difficulty (level - 1)
        // You might want to adjust this based on your spawner logic
        if (enemyRespawner != null)
        {
            enemyRespawner.increaseNumberENemy = lvlDifficulty + (level - 1); 
        }
    }
}
