using System.Collections.Generic;
using UnityEngine;

public class SpawnService : MonoBehaviour
{
    
    [SerializeField]private PlatformSpawner platformSpawner;
    [SerializeField]private List<GameObject> basicEnemyPrefabs;
    [SerializeField]private List<GameObject> enemyPrefabs;
    [SerializeField]private List<GameObject> powerupPrefabs;
    [SerializeField]private List<GameObject> scorePrefabs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        if (platformSpawner == null)
        {
        platformSpawner = GameObject.Find("PlatformSpawner").GetComponent<PlatformSpawner>();
           
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void SpawnAtRandomPosition(GameObject item)
    {
        Vector3 spawnPos = platformSpawner.GetRandomCenter();
        Instantiate(item, spawnPos, item.transform.rotation);
    }
    private void SpawnRandomItemFromList(List<GameObject> prefabList)
    {
        int randomIndex = Random.Range(0, prefabList.Count);
        SpawnAtRandomPosition(prefabList[randomIndex]);
    }
    public void SpawnRandomPowerup(int count = 1)
    {
        for (int i =0 ; i < count ; i++){

        SpawnRandomItemFromList(powerupPrefabs);
        }
    }
    public void SpawnRandomScore(int count = 1)
    {
        for (int i =0 ; i < count ; i++){

        SpawnRandomItemFromList(scorePrefabs);
        }
    }
    public void SpawnRandomEnemy(int count = 1)
    {
        for (int i =0 ; i < count ; i++){

        SpawnRandomItemFromList(enemyPrefabs);
        }
    }
    public void SpawnRandomBasicEnemy(int count = 1)
    {
        for (int i =0 ; i < count ; i++){

        SpawnRandomItemFromList(basicEnemyPrefabs);
        }
    }
    
}
