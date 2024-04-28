using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countdownText;
    private Animator _animator;
    private int _previousCountdownNumber;
    private const string NUMBER_POPUP = "NumberPopup";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    void Start()
    {
        GameManager.Instance.OnStateChanged += (sender, args) =>
        {
            if (GameManager.Instance.IsCountdownToStart())
            {
                Show();
            }
            else
            {
                Hide();
            }
        };
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        int countdownNUmber = Mathf.CeilToInt(GameManager.Instance.GetCountdownToStartTimer());
        _countdownText.text = countdownNUmber.ToString();
        if (_previousCountdownNumber != countdownNUmber)
        {
            _previousCountdownNumber = countdownNUmber;
            _animator.SetTrigger(NUMBER_POPUP);
            SoundManager.Instance.PlayCountdownSound();
        }
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
