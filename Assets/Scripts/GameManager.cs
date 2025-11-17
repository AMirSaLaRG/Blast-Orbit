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
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        enemyRespawner = GameObject.Find("EnemyRespawner").GetComponent<EnemyRespawner>();
        uiManager = GameObject.Find("InGameUIManager").GetComponent<InGameUIManager>();
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
