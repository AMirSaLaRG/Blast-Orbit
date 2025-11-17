using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawner : MonoBehaviour
{
    public GameObject bombPrefab;
    private PlatformSpawner platformSpawner;
    
    private const float TargetY = 1f;
    public int baseSummenNumber = 3;
    public float respawnInterval = 3f;
    public float spawnHeight = 40f;
    private int waweNumber = 1;
    public int increaseNumberENemy;

    [Header("Respawn Powerups Settings")]
    public List<GameObject> powerupPrefabs;
    private int maxPowerupValueToSummen = 1;
    public int powerupWawesEach = 5;
    public int powerupNumberToSummen = 1;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        platformSpawner = GameObject.Find("PlatformSpawner").GetComponent<PlatformSpawner>();
        StartCoroutine(NextSummenWave());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator NextSummenWave()
    {
        while (true)
        {
            SummenWave(baseSummenNumber + increaseNumberENemy);
            
            SummenPowerup(waweNumber, powerupNumberToSummen);

            waweNumber++;

            yield return new WaitForSeconds(respawnInterval);   
            
        }
    }
    private void SummenPowerup(int waweNumber, int respawnNumber)
    {
        Debug.Log(waweNumber+ powerupWawesEach+ (waweNumber % powerupWawesEach));
        if (waweNumber % powerupWawesEach == 0)
        {
            for (int i = 0; i < respawnNumber; i++)
            {
                Vector3 centerPos = platformSpawner.GetRandomCenter();
                Vector3 spawnPos = new Vector3(centerPos.x, 3, centerPos.z);
                int powerupToSummen = UnityEngine.Random.Range(0, powerupPrefabs.Count);
                Instantiate(powerupPrefabs[powerupToSummen], spawnPos, powerupPrefabs[powerupToSummen].transform.rotation);
            }
        }
        
    }
    private void SummenWave(int respawnNumber)
    {
        for (int i = 0; i < respawnNumber; i++)
        {
            RespawnBombTimer();
        }
    }
    public void RespawnBombTimer(Vector3? position = null)
    {
        Vector3 spawnPos;
        if (position == null)
        {
            Vector3 centerPos = platformSpawner.GetRandomCenter();
            
            spawnPos = new Vector3(centerPos.x, spawnHeight, centerPos.z);
        }
        else
        {
            spawnPos = position.Value;
        }
        

        GameObject newBomb = Instantiate(bombPrefab, spawnPos, Quaternion.identity);

        // Start the movement coroutine immediately after spawning (using a fixed 2.0s drop time)
        
    }
    
}
