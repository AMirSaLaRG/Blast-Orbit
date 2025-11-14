using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int level = 1;
    public int levelTime = 60;
    public int overalWawes;
    private EnemyRespawner enemyRespawner;
    private int numberToIncreaseLvlDifficulty = 3;
    private float remineTime;
    public bool isGameOver;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRespawner = GameObject.Find("EnemyRespawner").GetComponent<EnemyRespawner>();
        

    }

    // Update is called once per frame
    void Update()
    {
        remineTime = levelTime - Time.time;
        if (remineTime <= 0)
        {
            Debug.Log("🏆 Level Complete!");
            enemyRespawner.waweNumber = 0;
            level++;
        } else
        {
            Debug.Log(Time.time+ " / " + (levelTime / numberToIncreaseLvlDifficulty));
            int lvlDifficulty = (int)Math.Round(Time.time / (levelTime / numberToIncreaseLvlDifficulty));
            Debug.Log(lvlDifficulty);
            enemyRespawner.increaseNumberENemy = lvlDifficulty;
        }
        

    }
}
