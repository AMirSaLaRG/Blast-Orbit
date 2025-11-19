using UnityEngine; // Needed for JsonUtility
using System.IO;   // Needed for file saving

public static class DataPersistenceManager
{
    public static void SaveGame()
    {
        // 1. GET DATA: Create the serializable object and populate it
        UserSetting userSetting = UserSetting.Instance;
        GameData data = new GameData
        {
            username = userSetting.username,
            highscore = userSetting.highscore,
            cash = userSetting.cash,
            characterSpeedMultiplier = userSetting.characterSpeedMultiplier,
            characterGasAmountToAdd = userSetting.characterGasAmountToAdd,
            characterMaxHealth = userSetting.characterMaxHealth
        };

        string json = JsonUtility.ToJson(data);
        string path = Application.persistentDataPath + "/savefile.json";
        File.WriteAllText(path, json);
    }
    public static void LoadGame()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            GameData data = JsonUtility.FromJson<GameData>(json);

            UserSetting userSetting = UserSetting.Instance;
            userSetting.username = data.username;
            userSetting.highscore = data.highscore;
            userSetting.cash = data.cash;
            userSetting.characterSpeedMultiplier = data.characterSpeedMultiplier;
            userSetting.characterGasAmountToAdd = data.characterGasAmountToAdd;
            userSetting.characterMaxHealth = data.characterMaxHealth;
        }
    }
}