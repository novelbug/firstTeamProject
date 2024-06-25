using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameObject container;

    static DataManager instance;
    public static DataManager Instance
    {
        get
        {
            if (!instance)
            {
                container = new GameObject();
                container.name = "DataManager";
                instance = container.AddComponent(typeof(DataManager)) as DataManager;
                DontDestroyOnLoad(container);
            }
            return instance;
        }
    }

    public string playerName;

    public int maxGameScore = 0; 
    public int gameScore = 0;

    private void Awake()
    {
        DataManager.Instance.LoadGameData();
    }

    private void OnApplicationQuit()
    {
        DataManager.Instance.SaveGameData();
    }
}
