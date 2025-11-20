using UnityEngine;
[System.Serializable]
public class GameData
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string username;
    public int highscore;
    public int cash;
    public float characterSpeedMultiplier;
    public float characterGasAmountToAdd;
    public int characterMaxHealthToAdd;
    internal UserSetting userSetting;

    void SaveGame()
    {
        
    }
}
