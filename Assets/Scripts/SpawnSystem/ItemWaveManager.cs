using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class ItemWaveManager : MonoBehaviour
{
    [SerializeField]int waweTime = 5;
    public int waweNumber {get; private set;} = 1;
    [SerializeField]private GameManager gameManager;
    [SerializeField] private SpawnService spawnService;
    [SerializeField] private WaweDifficultyManager waweDifficultyManager;
    
    [Header ("Wawe Manager")]
    [SerializeField] private int basicNumberOfBombRespawnAtWawe = 1;
    private int currentNumberOfBombRespawnAtWawe;
    [SerializeField] private int basicNumberOfBasicBombRespawnAtWawe = 1;
    private int currentNumberOfBasicBombRespawnAtWawe;
    [SerializeField] private int basicNumberOfPowerupRespawnAtWawe = 1;
    private int currentNumberOfPowerupRespawnAtWawe;
    [SerializeField] private int basicNumberOfScoreRespawnAtWawe = 1;
    private int currentNumberOfScoreRespawnAtWawe;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        if (spawnService == null)
        {
            spawnService = GameObject.Find("SpawnService").GetComponent<SpawnService>();
        }
        currentNumberOfBombRespawnAtWawe = basicNumberOfBombRespawnAtWawe;
        currentNumberOfBasicBombRespawnAtWawe = basicNumberOfBasicBombRespawnAtWawe;
        currentNumberOfPowerupRespawnAtWawe = basicNumberOfPowerupRespawnAtWawe;
        currentNumberOfScoreRespawnAtWawe = basicNumberOfScoreRespawnAtWawe;
        StartCoroutine(NextWawe());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }    
    private IEnumerator NextWawe()
    {
        Debug.Log("this shit goes down");
        while (!gameManager.isGameOver)
        {
            
            waweNumber ++;
            waweDifficultyManager.CheckForUpdatesOnDifficulty();
            spawnService.SpawnRandomBasicEnemy(currentNumberOfBasicBombRespawnAtWawe);
            spawnService.SpawnRandomEnemy(currentNumberOfBombRespawnAtWawe);
            spawnService.SpawnRandomPowerup(currentNumberOfPowerupRespawnAtWawe);
            spawnService.SpawnRandomScore(currentNumberOfScoreRespawnAtWawe);
            yield return new WaitForSeconds(waweTime);
        }
        
    }
    public void IncreaseAmountOfBombEachWawe(int num = 1)
    {
        currentNumberOfBombRespawnAtWawe += num;
    }
    public void IncreaseAmountOfBasicBombEachWawe(int num = 1)
    {
        currentNumberOfBasicBombRespawnAtWawe += num;
    }
    public void IncreaseAmountOfPowerupEachWawe(int num = 1)
    {
        currentNumberOfPowerupRespawnAtWawe += num;
    }
    public void IncreaseAmountOfScoreEachWawe(int num = 1)
    {
        currentNumberOfScoreRespawnAtWawe += num;
    }





}
