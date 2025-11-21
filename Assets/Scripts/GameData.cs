using UnityEngine;
[System.Serializable]
public class GameData
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string username;
    public int highscore;
    public int cash;
    public float characterSpeedLvl;
    public float characterGasLvl;
    public int characterMaxHealthLvl;
    public int highScoreLvl;
    public bool isUser;
    internal UserSetting userSetting;

    void SaveGame()
    {
        
    }
}
