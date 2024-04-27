using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button _resumeBtn;
    [SerializeField] private Button _mainMenuBtn;
    [SerializeField] private Button _optionBtn;
    [SerializeField] private OptionUI _optionUI;

    private void Awake()
    {
        _resumeBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.TogglePauseGame();
        });

        _mainMenuBtn.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });

        _optionBtn.onClick.AddListener(() =>
        {
            _optionUI.Show();
            Hide();
        });

        _optionUI.OnClose += (sender, args) =>
        {
            Show();
        };
    }

    private void Start()
    {
        GameManager.Instance.OnGamePaused += (sender, args) =>
        {
            Show();
        };
        GameManager.Instance.OnGameUnpaused += (sender, args) =>
        {
            Hide();
        };

        Hide();
    }
    private void Show()
    {
        gameObject.SetActive(true);
        _optionBtn.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
