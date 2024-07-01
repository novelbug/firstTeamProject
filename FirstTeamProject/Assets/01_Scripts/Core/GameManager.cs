using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameOverReasons
{
    Fall, Obstacle, GiveUp
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup _gameOverPanel;
    [SerializeField] private Button _lobbyButton;
    [SerializeField] private CanvasGroup _escapePanel;
    [SerializeField] private Button _giveupButton;
    [SerializeField] private Button _returnButton;
    [SerializeField] private Button _replayButton;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TextMeshProUGUI _playerNameText;

    public static GameManager Instance;

    public string playerName;

    public bool isGameOver = false;
    public bool isGameStopped = false;
    public int maxGameScore = 0; 
    public int gameScore = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        _lobbyButton.onClick.AddListener(ReturnToLobby);
        _giveupButton.onClick.AddListener(() =>
        {
            GameOver(GameOverReasons.GiveUp);
        });
        _returnButton.onClick.AddListener(ReturnToPlay);
        _replayButton.onClick.AddListener(() =>
        {
            DataManager.Instance.data.PlayerName = playerName;
            DataManager.Instance.data.PlayerRecentGameScore = gameScore;
            DataManager.Instance.data.PlayerMaxGameScore = maxGameScore;

            Loading.LoadScene(SceneManager.GetActiveScene().name);
        });

        isGameOver = false;
        Time.timeScale = 1;

        playerName = DataManager.Instance.data.PlayerName;
        if (playerName != null) _playerNameText.text = $"{playerName}";
    }

    private void OnApplicationQuit()
    {
        DataManager.Instance.SaveGameData(playerName, gameScore, maxGameScore);
    }

    private void Update()
    {
        Debug.Log(gameObject.name);
        _scoreText.text = gameScore.ToString();

        if(!isGameOver && Input.GetKeyDown(KeyCode.Escape))
        {
            Escape(!_escapePanel.interactable);
        }
    }

    private void ReturnToPlay()
    {
        Escape(false);
    }

    private void Escape(bool value)
    {
        SetActive(_escapePanel, value);

        Time.timeScale = value ? 0 : 1;
    }

    public void GameOver(GameOverReasons reason)
    {
        isGameOver = true;
        Time.timeScale = 0;

        if (_escapePanel.interactable) SetActive(_escapePanel, false);

        _gameOverPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            reason == GameOverReasons.Fall ? "낙사했습니다..." : (reason == GameOverReasons.Obstacle ? "장애물에 부딪혔습니다..." : "포기했습니다...");
        SetActive(_gameOverPanel, true);
        
        if (gameScore > maxGameScore)
            maxGameScore = gameScore;
    }

    private void ReturnToLobby()
    {
        DataManager.Instance.SaveGameData(playerName, gameScore, maxGameScore);
        Loading.LoadScene("Start");
    }

    private void SetActive(CanvasGroup group, bool value = true)
    {
        group.alpha = value ? 1 : 0;
        group.interactable = value;
        group.blocksRaycasts = value;
    }

    private void OnDestroy()
    {
        DataManager.Instance.data.PlayerName = playerName;
        DataManager.Instance.data.PlayerRecentGameScore = gameScore;
        DataManager.Instance.data.PlayerMaxGameScore = maxGameScore;
    }
}
