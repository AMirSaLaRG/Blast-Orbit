using System.Collections;
using UnityEngine;

public class EnemyRespawner : MonoBehaviour
{
    public GameObject bombPrefab;
    private PlatformSpawner platformSpawner;
    
    private const float TargetY = 1f;
    public int baseSummenNumber = 3;
    public float respawnInterval = 3f;
    public float spawnHeight = 40f;
    public int waweNumber;
    public int increaseNumberENemy;
    
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
            waweNumber++;
            yield return new WaitForSeconds(respawnInterval);   
            
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
