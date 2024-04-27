using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button _resumeBtn;
    [SerializeField] private Button _mainMenuBtn;

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
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
