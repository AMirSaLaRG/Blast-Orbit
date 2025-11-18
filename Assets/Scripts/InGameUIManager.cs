using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using Microsoft.Unity.VisualStudio.Editor;

public class InGameUIManager : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI cashText;
    private GameManager gameManager;
    public UnityEngine.UI.Image gasBar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
    
    }
    public void SetTimeText(string text)
    {
        timeText.text = text;
    }
    public void SetLevelText(string text)
    {
        levelText.text = text;
    }
    public void SetHealthText(int healthNumber)
    {
        string healthIcon = " ";
        for (int i = 0; i < healthNumber; i++)
        {
            healthIcon += " ♥";
        }   
        healthText.text = "Health: " + healthIcon;
    }
    public void setGasBar(int currentGas, int maxGas)
    {
        gasBar.fillAmount = (float)currentGas / maxGas;
    }  
    public void SetCashText(int cashNumber)
    {
        cashText.text = "Cash: $" + cashNumber.ToString();
    }   
    
}
