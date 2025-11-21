using UnityEngine;

public class UserSetting : MonoBehaviour
{
    public static UserSetting Instance;

    public bool isUser = false;
    public string username;
    public int highscore = 0;
    public int cash = 0;
    public int highScoreLvl;
    [Header ("Speed")]
    public float characterSpeedLvl = 0f;
    [Header("Gas")]
    public float characterGasLvl = 0f;
    [Header("Health")]
    public int characterMaxHealthLvl = 0;
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

}
