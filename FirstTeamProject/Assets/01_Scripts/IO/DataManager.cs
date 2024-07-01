using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DataManager : MonoBehaviour
{
    static GameObject container;

    static DataManager instance;
    public static DataManager Instance
    {
        get
        {
            if(!instance)
            {
                container = new GameObject();
                container.name = "DataManager";
                instance = container.AddComponent(typeof(DataManager)) as DataManager;
                DontDestroyOnLoad(container);
            }
            return instance;
        }
    }

    string GameDataPath = "C:/Program Files/InfinityEnergy_CatBread";
    string GameDataFileName = "GameData.json";

    public Data data = new Data();

    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + "/" + GameDataFileName;

        if (File.Exists(filePath))
        {
            string FromJsonData = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<Data>(FromJsonData);

            Console.WriteLine("Load Complete!");
        }
    }

    public void SaveGameData(string name, int score, int maxScore)
    {
        data.PlayerName = name;
        data.PlayerRecentGameScore = score;
        data.PlayerMaxGameScore = maxScore;

        string ToJsonData = JsonUtility.ToJson(data, true);
        string filePath = GameDataPath + "/" + GameDataFileName;

        File.WriteAllText(filePath, ToJsonData);

        Console.WriteLine("Save Complete!");
    }

    private void OnApplicationQuit()
    {
        if(GameManager.Instance != null)
        {
            SaveGameData(GameManager.Instance.playerName,
                GameManager.Instance.gameScore, GameManager.Instance.maxGameScore);
        }
        else
        {
            SaveGameData(data.PlayerName, data.PlayerRecentGameScore, data.PlayerMaxGameScore);
        }
    }
}
