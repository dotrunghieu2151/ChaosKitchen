using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public event EventHandler OnClose;
    [SerializeField] private Button _soundEffectBtn;
    [SerializeField] private Button _musicBtn;
    [SerializeField] private Button _closeBtn;

    [SerializeField] private Button _moveUpBtn;

    [SerializeField] private Button _moveDownBtn;

    [SerializeField] private Button _moveLeftBtn;

    [SerializeField] private Button _moveRightBtn;

    [SerializeField] private Button _interactBtn;

    [SerializeField] private Button _interactAlternateBtn;

    [SerializeField] private Button _pauseBtn;

    [SerializeField] private TextMeshProUGUI _soundEffectText;
    [SerializeField] private TextMeshProUGUI _musicText;

    [SerializeField] private TextMeshProUGUI _moveUpText;
    [SerializeField] private TextMeshProUGUI _moveDownText;
    [SerializeField] private TextMeshProUGUI _moveLeftText;
    [SerializeField] private TextMeshProUGUI _moveRightText;

    [SerializeField] private TextMeshProUGUI _interactText;

    [SerializeField] private TextMeshProUGUI _interactAlternateText;

    [SerializeField] private TextMeshProUGUI _pausesText;

    [SerializeField] private Transform _pressToRebindTransfor;

    public void Show()
    {
        gameObject.SetActive(true);
        _soundEffectBtn.Select();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        _soundEffectBtn.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        _musicBtn.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        _closeBtn.onClick.AddListener(() =>
        {
            Hide();
            OnClose?.Invoke(this, EventArgs.Empty);
        });

        _moveUpBtn.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.MoveUp);
        });

        _moveDownBtn.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.MoveDown);
        });

        _moveLeftBtn.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.MoveLeft);
        });

        _moveRightBtn.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.MoveRight);
        });

        _interactBtn.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Interact);
        });

        _interactAlternateBtn.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.InteractAlternate);
        });

        _pauseBtn.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Pause);
        });
    }

    private void Start()
    {
        GameManager.Instance.OnGameUnpaused += (sender, args) =>
        {
            Hide();
        };
        UpdateVisual();
        Hide();
        HidePressToRebindKey();
    }

    private void UpdateVisual()
    {
        _soundEffectText.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
        _musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);

        _moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp);
        _moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown);
        _moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft);
        _moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight);
        _interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        _interactAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        _pausesText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    }

    private void ShowPressToRebindKey()
    {
        _pressToRebindTransfor.gameObject.SetActive(true);
    }

    private void HidePressToRebindKey()
    {
        _pressToRebindTransfor.gameObject.SetActive(false);
    }

    private void RebindBinding(GameInput.Binding binding)
    {
        ShowPressToRebindKey();
        GameInput.Instance.RebindBinding(binding, () =>
        {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }
}
