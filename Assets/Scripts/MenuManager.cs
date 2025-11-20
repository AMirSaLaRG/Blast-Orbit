using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    public TextMeshProUGUI userNameText;
    public GameObject userNameInput;
    private UserSetting userSetting;
   
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
        if (userSetting != null && userSetting.username != null)
        {
            userNameInput.SetActive(false);
        } else
        {
            userNameText.enabled = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        DisplaySpeedField(requestToAddSpeed);
        DisplayGasField(requestToAddGas);
        DisplayHealthField(requestToAddHealth);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
    // Speed
    private void DisplaySpeedField(float requestToAdd = 0)
    {
        float updatePersentageGas = (userSetting.characterSpeedMultiplier + requestToAdd) ;
        speedUpgradeNumberText.text = updatePersentageGas + "%";
        speedUpgradePriceText.text = "Price: " + (requestToAdd * cashEachUpgradeSpeed);
        
    }
    public void TrytoUpdateBaseSpeed(float requestToAdd = 0)
    {
        float upgradePercantage = requestToAdd * 0.01f;
        float cost = requestToAdd * cashEachUpgradeSpeed;
        int cash = userSetting.cash;

        if (cash < cost) return;
        
            cash -= (int)cost;

        userSetting.characterSpeedMultiplier -= upgradePercantage;
        DataPersistenceManager.SaveGame();
        
    }
    public void RequestToAddSpeedPlus()
    {        

        if ((userSetting.characterSpeedMultiplier + requestToAddSpeed) == maxAmountOfAllowSpeedUpdate) return;
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
        float updatePersentageGas = (userSetting.characterGasAmountToAdd + requestToAdd);
        gasUpgradeNumberText.text = updatePersentageGas + "%";
        gasUpgradePriceText.text = "Price: " + (requestToAdd * cashEachUpgradeGas);
        
    }
    public void TrytoUpdateBaseGass(float requestToAdd = 0)
    {
        float cost = requestToAdd * cashEachUpgradeGas;
        int cash = userSetting.cash;

        if (cash < cost) return;
        
            cash -= (int)cost;

        userSetting.characterGasAmountToAdd += requestToAdd;
        DataPersistenceManager.SaveGame();
        }
    public void RequestToAddGasPlus()
    {
        if ((userSetting.characterGasAmountToAdd + requestToAddGas) == maxAmountOfAllowGasUpdate) return;
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
        float updatePersentageGas = (userSetting.characterMaxHealthToAdd + requestToAdd) / maxAmountOfAllowHealthUpdate * 100; 
        healthUpgradeNumberText.text = updatePersentageGas.ToString("F0") + "%";
        healthUpgradePriceText.text = "Price: " + (requestToAdd * cashEachUpgradeHealth);
        
    }
    public void TrytoUpdateBaseHealth(int requestToAdd = 0)
    {
        float cost = requestToAdd * cashEachUpgradeHealth;
        int cash = userSetting.cash;

        if (cash < cost) return;
        
            cash -= (int)cost;

        userSetting.characterMaxHealthToAdd += requestToAdd;
        DataPersistenceManager.SaveGame();
        }
    
    public void RequestToAddHealthPlus()
    {
        if ((userSetting.characterMaxHealthToAdd + requestToAddHealth) == maxAmountOfAllowHealthUpdate) return;
        requestToAddHealth += 1;
    }
    public void RequestToAddHealthMinus()
    {
        if (requestToAddHealth == 0) return;
        
        requestToAddHealth -= 1;
        
    }

}
