using UnityEngine;

public class UserSetting : MonoBehaviour
{
    public static UserSetting Instance;

    public string username = "NewPlayer";
    public int highscore = 0;
    public int cash = 0;
    [Header ("Speed")]
    public float characterSpeedMultiplier = 0f;
    public float cashEachUpgradeSpeed = 1f;
    [Header("Gas")]
    public float characterGasAmountToAdd = 0f;
    public float cashEachUpgradeGas = 5f;
    [Header("Health")]
    public int characterMaxHealthToAdd = 0;
    public float cashEachUpgradeHealth = 100f;
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
        DataPersistenceManager.LoadGame();
    }
    public void TrytoUpdateBaseSpeed(int upgrade)
    {
        float upgradePercantage = upgrade * 0.01f;
        float cost = upgrade * cashEachUpgradeSpeed;

        if (cash < cost) return;
        
            cash -= (int)cost;

        characterSpeedMultiplier += upgradePercantage;
        DataPersistenceManager.SaveGame();
        
    }
    public void TrytoUpdateBaseGass(int upgrade)
    {
        float cost = upgrade * cashEachUpgradeGas;

        if (cash < cost) return;
        
            cash -= (int)cost;

        characterGasAmountToAdd += upgrade;
        DataPersistenceManager.SaveGame();
        }
    
    public void TrytoUpdateBaseHealth(int upgrade)
    {
        float cost = upgrade * cashEachUpgradeHealth;

        if (cash < cost) return;
        
            cash -= (int)cost;

        characterMaxHealthToAdd += upgrade;
        DataPersistenceManager.SaveGame();
        }
    
}
