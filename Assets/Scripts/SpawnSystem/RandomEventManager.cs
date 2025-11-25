using System.Collections;
using UnityEngine;

public abstract class RandomEventManager : MonoBehaviour
{
    [SerializeField]private GameManager gameManager;
    [SerializeField]protected int basicChance = 5;
    [SerializeField]protected float basicTime = 0.4f;
    [SerializeField]protected int basicLevel = 1;
    [SerializeField]protected float timeReductionPerLevel = 0.01f;
    [SerializeField]protected int levelsPerChanceIncrease = 20;
    public int currenctLevel {get; private set;}
    private  int currentChance;
    private float currentTime;
    [SerializeField]private SpawnService spawnService;
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
        currentChance = basicChance;
        currentTime = basicTime;
        currenctLevel=basicLevel;
        StartCoroutine(RandomEventLoop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void RandomSummener(int oneInChance)
    {
        int luckNumber = UnityEngine.Random.Range(0, oneInChance);
        if (luckNumber == 0)
        {
            SpecialRespawn(spawnService);
        }
    }
    private IEnumerator RandomEventLoop()
    {
        while (!gameManager.isGameOver)
        {
            RandomSummener(currentChance);
            yield return new WaitForSeconds(currentTime);   
        }
        
        
    }
    public void IncreaseChance(int num = 1)
    {   
        if (currentTime <= 0) return;
        currentTime -= (num * timeReductionPerLevel);
        currenctLevel++;
        if (currenctLevel % levelsPerChanceIncrease == 0)
        {
            currentChance -= 1;
            currentTime = basicTime;
        }
    }
    protected abstract void SpecialRespawn(SpawnService spawnService);
}
