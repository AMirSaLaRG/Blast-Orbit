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
            // characterSpeedLvl = userSetting.characterSpeedLvl,
            // characterGasLvl = userSetting.characterGasLvl,
            // characterMaxHealthLvl = userSetting.characterMaxHealthLvl,
            highScoreLvl = userSetting.highScoreLvl,
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
            Debug.Log("file exist and " + UserSetting.Instance.isUser);
            string json = File.ReadAllText(path);
            GameData data = JsonUtility.FromJson<GameData>(json);

            UserSetting userSetting = UserSetting.Instance;
            userSetting.username = data.username;
            userSetting.highscore = data.highscore;
            userSetting.cash = data.cash;
            // userSetting.characterSpeedLvl = data.characterSpeedLvl;
            // userSetting.characterGasLvl = data.characterGasLvl;
            // userSetting.characterMaxHealthLvl = data.characterMaxHealthLvl;
            userSetting.highscore = data.highscore;
            userSetting.isUser = data.isUser;
        }
    }
}