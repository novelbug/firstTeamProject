using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{
    [SerializeField] private CanvasGroup _startPanel;
    [SerializeField] private CanvasGroup _exitPanel;
    [SerializeField] private TMP_InputField _nameInputField;
    [SerializeField] private Button _loginStartBtn;
    [SerializeField] private Button _logupStartBtn;
    [SerializeField] private Button _exitBtn;
    [SerializeField] private Button _returnBtn;
    [SerializeField] private TextMeshProUGUI _warningText;

    private void Awake()
    {
        DataManager.Instance.LoadGameData();

        _loginStartBtn.onClick.AddListener(Login);
        _logupStartBtn.onClick.AddListener(Logup);
        _exitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        _returnBtn.onClick.AddListener(() =>
        {
            SetAlpha(_exitPanel, false);
        });

        SetAlpha(_exitPanel, false);
        SetAlpha(_startPanel, false);
        _warningText.alpha = 0;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (_startPanel.interactable) SetAlpha(_startPanel, false);
            else
            {
                if (_exitPanel.interactable) SetAlpha(_exitPanel, false);
                else SetAlpha(_exitPanel, true);
            }
        }
        else if(Input.anyKeyDown)
        {
            if (!_startPanel.interactable && !_exitPanel.interactable)
            {
                SetAlpha(_startPanel, true);
            }
        }
    }

    private void Login()
    {
        DataManager.Instance.LoadGameData();
        string playerName = DataManager.Instance.data.PlayerName;
        if (playerName == "" || playerName == null)
        {
            WarningText("저장된 이름이 없습니다");
            return;
        }

        Debug.Log("이어하기 : " + playerName);
        Loading.LoadScene("PlayScene");
    }

    private void Logup()
    {
        string inputName = _nameInputField.text;
        if (inputName == string.Empty)
        {
            WarningText("이름을 설정해주세요");
            return;
        }

        Debug.Log("새로 시작하기 : " + inputName);

        DataManager.Instance.data.PlayerName = inputName;
        Loading.LoadScene("PlayScene");
    }

    private void WarningText(string value)
    {
        _warningText.text = value;
        Sequence seq = DOTween.Sequence();
        seq.Append(DOVirtual.Float(0, 1f, 0.1f, val => _warningText.alpha = val));
        seq.AppendInterval(0.25f);
        seq.Append(DOVirtual.Float(1, 0f, 1f, val => _warningText.alpha = val));
    }

    private void SetAlpha(CanvasGroup canvas, bool value = true)
    {
        canvas.alpha = value ? 1 : 0;
        canvas.interactable = value;
        canvas.blocksRaycasts = value;
    }
}
