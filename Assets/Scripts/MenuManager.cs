using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    
    private UserSetting userSetting;
    [Header ("User Info")]
    public TextMeshProUGUI userNameInputField;
    public TextMeshProUGUI userNameText;
    public GameObject userNameInput;
    public TextMeshProUGUI cashText;
    public TextMeshProUGUI MaxLvlText;
   
    [Header ("Speed")]
    public float characterSpeedMultiplier;
    public float cashEachUpgradeSpeed = 5f;
    public float maxAmountOfAllowSpeedUpdate = 100;
    public TextMeshProUGUI speedUpgradeNumberText;
    public TextMeshProUGUI speedUpgradePriceText;
    
    private float requestToAddSpeed = 0;
    [Header("Gas")]
    public float characterGasAmountToAdd;
    public float cashEachUpgradeGas = 5f;
    public float maxAmountOfAllowGasUpdate = 100;
    public TextMeshProUGUI gasUpgradeNumberText;
    public TextMeshProUGUI gasUpgradePriceText;
    private float requestToAddGas = 0;
    [Header("Health")]
    public int characterMaxHealthToAdd;
    public float cashEachUpgradeHealth = 100f;
    public float maxAmountOfAllowHealthUpdate = 3;
    private float requestToAddHealth = 0;
    public TextMeshProUGUI healthUpgradeNumberText;
    public TextMeshProUGUI healthUpgradePriceText;
  // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        userSetting = UserSetting.Instance;
        // Debug.Log(Application.persistentDataPath);
        
    }

    // Update is called once per frame
    void Update()
    {
        // DisplaySpeedField(requestToAddSpeed);
        // DisplayGasField(requestToAddGas);
        // DisplayHealthField(requestToAddHealth);
        DisplayCashAndLevelAndUsername();
    }

    public void StartGame()
    {
        if (!userSetting.isUser)
        {
            string userName = userNameInputField.text;
            userSetting.username = userName;
            userSetting.isUser = true;
            DataPersistenceManager.SaveGame();
            
        }
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
    private void DisplayCashAndLevelAndUsername()
    {
        userSetting = UserSetting.Instance;
        if (userSetting.isUser)
        {
            
            userNameInput.SetActive(false);
            userNameText.text = userSetting.username;
            
        } else
        { 
            userNameText.enabled = false;
            
            
        }
        cashText.text = "Cash: $" + userSetting.cash;
        MaxLvlText.text = "Max Level: " + userSetting.highScoreLvl;
    }
    // Speed
    private void DisplaySpeedField(float requestToAdd = 0)
    {
        float updatePersentageSpeed = (userSetting.characterSpeedLvl + requestToAdd) ;
        speedUpgradeNumberText.text = updatePersentageSpeed + "%";
        speedUpgradePriceText.text = "Price: " + (requestToAdd * cashEachUpgradeSpeed);
        
    }
    public void TrytoUpdateBaseSpeed()
    {
        float cost = requestToAddSpeed * cashEachUpgradeSpeed;
        int cash = userSetting.cash;

        Debug.Log("checking cash");

        if (cash < cost || requestToAddSpeed == 0) return;
        
        Debug.Log("enough cash");
        
        cash -= (int)cost;
        userSetting.cash = cash;

        
        userSetting.characterSpeedLvl += requestToAddSpeed;
        requestToAddSpeed = 0;
        DataPersistenceManager.SaveGame();
        
    }
    public void RequestToAddSpeedPlus()
    {        

        if ((userSetting.characterSpeedLvl + requestToAddSpeed) == maxAmountOfAllowSpeedUpdate) return;
        requestToAddSpeed += 1;
    }
    public void RequestToAddSpeedMinus()
    {
        if (requestToAddSpeed == 0) return;
        requestToAddSpeed -= 1;
    }
    // Gas
    private void DisplayGasField(float requestToAdd = 0)
    {
        float updatePersentageGas = (userSetting.characterGasLvl + requestToAdd);
        gasUpgradeNumberText.text = updatePersentageGas + "%";
        gasUpgradePriceText.text = "Price: " + (requestToAdd * cashEachUpgradeGas);
        
    }
    public void TrytoUpdateBaseGass()
    {
        float cost = requestToAddGas * cashEachUpgradeGas;
        int cash = userSetting.cash;

        Debug.Log("checking cash");

        if (cash < cost || requestToAddGas == 0) return;
        
        Debug.Log("enough cash");
        
        cash -= (int)cost;
        userSetting.cash = cash;

        userSetting.characterGasLvl += requestToAddGas;
        requestToAddGas = 0;

        DataPersistenceManager.SaveGame();
        }
    public void RequestToAddGasPlus()
    {
        if ((userSetting.characterGasLvl + requestToAddGas) == maxAmountOfAllowGasUpdate) return;
        requestToAddGas += 1;
    }
    public void RequestToAddGasMinus()
    {
        if (requestToAddGas == 0) return;
        requestToAddGas -= 1;
    }
    // Health
    private void DisplayHealthField(float requestToAdd = 0)
    {
        float updatePersentageGas = (userSetting.characterMaxHealthLvl + requestToAdd) / maxAmountOfAllowHealthUpdate * 100; 
        healthUpgradeNumberText.text = updatePersentageGas.ToString("F0") + "%";
        healthUpgradePriceText.text = "Price: " + (requestToAdd * cashEachUpgradeHealth);
        
    }
    public void TrytoUpdateBaseHealth()
    {
        float cost = requestToAddHealth * cashEachUpgradeHealth;
        int cash = userSetting.cash;


        if (cash < cost || requestToAddHealth == 0) return;
        
        Debug.Log("enough cash");


        cash -= (int)cost;
        userSetting.cash = cash;


        
        userSetting.characterMaxHealthLvl += (int)requestToAddHealth;
        requestToAddHealth = 0;
        DataPersistenceManager.SaveGame();
        }
    
    public void RequestToAddHealthPlus()
    {
        if ((userSetting.characterMaxHealthLvl + requestToAddHealth) == maxAmountOfAllowHealthUpdate) return;
        requestToAddHealth += 1;
    }
    public void RequestToAddHealthMinus()
    {
        if (requestToAddHealth == 0) return;
        
        requestToAddHealth -= 1;
        
    }

}
