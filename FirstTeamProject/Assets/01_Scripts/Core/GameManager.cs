using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;

    static GameObject container;
    public static GameManager Instance;

    public string playerName;

    public int maxGameScore = 0; 
    public int gameScore = 0;

    private void Awake()
    {
        DataManager.Instance.LoadGameData();
        Instance = this;
        if (Instance != null) Destroy(this);

    }

    private void OnApplicationQuit()
    {
        DataManager.Instance.SaveGameData();
    }

    private void Update()
    {
        Debug.Log(gameObject.name);
        if (_scoreText == null) Debug.Log("WWWWWWWWWW");
        _scoreText.text = gameScore.ToString();
    }

    public void GameOver()
    {
        if (gameScore > maxGameScore)
            maxGameScore = gameScore;
    }
}
