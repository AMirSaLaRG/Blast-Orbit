using UnityEngine;

public class WaweDifficultyManager : MonoBehaviour
{
    [SerializeField]private ItemWaveManager itemWaveManager;
    [Header ("Basic Bombs")]
    [SerializeField]private RandomBasicBombRespawner randomBasicBombRespawner;
    [SerializeField]private int multiplyOfWaweToIncreaseLevelOfBasicBomb;
    [SerializeField]private int basicLevelIncreaseOfBasicBomb = 1;
    [Header ("Powerups")]
    [SerializeField]private RandomPowerupRespawner randomPowerupRespawner;
    [SerializeField]private int multiplyOfWaweToIncreaseLevelOfPowerup;
    [SerializeField]private int basicLevelIncreaseOfPowerup = 1;
    [Header ("Score")]
    [SerializeField]private RandomScoreRespawner randomScoreRespawner;
    [SerializeField]private int multiplyOfWaweToIncreaseLevelOfScore;
    [SerializeField]private int basicLevelIncreaseOfScore = 1;
    [Header ("Wawe respawn BasicBomb")]
    [SerializeField]private int multiplyofWaweToIncreaseBasicBomb;
    [SerializeField]private int numToIncreaseBasicBomb = 1;
    [Header ("Wawe respawn Bomb")]
    [SerializeField]private int multiplyofWaweToIncreaseBomb;
    [SerializeField]private int numToIncreaseBomb = 1;
    [Header ("Wawe respawn Powerup")]
    [SerializeField]private int multiplyofWaweToIncreasePowerup;
    [SerializeField]private int numToIncreasePowerup = 1;
    [Header ("Wawe respawn Score")]
    [SerializeField]private int multiplyofWaweToIncreaseScore;
    [SerializeField]private int numToIncreaseScore = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (itemWaveManager == null)
        {
            itemWaveManager = GameObject.Find("ItemWaveManager").GetComponent<ItemWaveManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void CheckForUpdatesOnDifficulty()
    {
        CheckToIncreaseLevelOfBasicBomb();
        CheckToIncreaseLevelOfPowerup();
        CheckToIncreaseLevelOfScore();
        CheckToIncreaseWaweLevelOfBasicBomb();
        CheckToIncreaseWaweLevelOfBomb();
        CheckToIncreaseWaweLevelOfPowerup();
        CheckToIncreaseWaweLevelOfScore();
    }
    private bool IsItTheWawe(int num, bool summenEachNum)
    {
        int waweNumber = itemWaveManager.waweNumber;
        if (summenEachNum)
        {
            if (waweNumber % num == 0)
            {
                return true;
            }
            
        } else
        {
           if (waweNumber == num)
            {
                return true;
            } 
        }
        return false;
    }
    private void CheckToIncreaseLevelOfBasicBomb()
    {
        if (IsItTheWawe(multiplyOfWaweToIncreaseLevelOfBasicBomb, true))
        {
            randomBasicBombRespawner.IncreaseChance(basicLevelIncreaseOfBasicBomb);
        }
    }
    private void CheckToIncreaseLevelOfPowerup()
    {
    
        if (IsItTheWawe(multiplyOfWaweToIncreaseLevelOfPowerup, true))
        {
            randomPowerupRespawner.IncreaseChance(basicLevelIncreaseOfPowerup);
        }
    
    }
    private void CheckToIncreaseLevelOfScore()
    {
        if (IsItTheWawe(multiplyOfWaweToIncreaseLevelOfScore, true))
        {
            randomScoreRespawner.IncreaseChance(basicLevelIncreaseOfScore);
        }
    }
    private void CheckToIncreaseWaweLevelOfBasicBomb()
    {
        if (IsItTheWawe(multiplyofWaweToIncreaseBasicBomb, true))
        {
            randomScoreRespawner.IncreaseChance(numToIncreaseBasicBomb);
        }
    }
    private void CheckToIncreaseWaweLevelOfBomb()
    {
        if (IsItTheWawe(multiplyofWaweToIncreaseBomb, true))
        {
            randomScoreRespawner.IncreaseChance(numToIncreaseBomb);
        }
    }
    private void CheckToIncreaseWaweLevelOfPowerup()
    {
        if (IsItTheWawe(multiplyofWaweToIncreasePowerup, true))
        {
            randomScoreRespawner.IncreaseChance(numToIncreasePowerup);
        }
    }
    private void CheckToIncreaseWaweLevelOfScore()
    {
        if (IsItTheWawe(multiplyofWaweToIncreaseScore, true))
        {
            randomScoreRespawner.IncreaseChance(numToIncreaseScore);
        }
    }

}
