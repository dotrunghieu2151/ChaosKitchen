using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event EventHandler OnStateChanged;
    public static GameManager Instance { get; private set; }
    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    private State _state;

    [SerializeField] private float _waitingToStartTimer = 1f;
    [SerializeField] private float _countdownToStartTimer = 3f;

    private float _gamePlayTimer;
    [SerializeField] private float _gamePlayTimerMax = 10f;

    public bool IsGamePlaying()
    {
        return _state == State.GamePlaying;
    }

    public bool IsCountdownToStart()
    {
        return _state == State.CountdownToStart;
    }

    public bool IsGameOver()
    {
        return _state == State.GameOver;
    }

    public float GetCountdownToStartTimer()
    {
        return _countdownToStartTimer;
    }

    public float GetGamePlayingTimerNormalize()
    {
        return 1 - (_gamePlayTimer / _gamePlayTimerMax);
    }
    private void Awake()
    {
        Instance = this;
        _state = State.WaitingToStart;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case State.WaitingToStart:
                {
                    _waitingToStartTimer -= Time.deltaTime;
                    if (_waitingToStartTimer <= 0f)
                    {
                        _state = State.CountdownToStart;
                        OnStateChanged?.Invoke(this, EventArgs.Empty);
                    }
                    break;
                }

            case State.CountdownToStart:
                {
                    _countdownToStartTimer -= Time.deltaTime;
                    if (_countdownToStartTimer <= 0f)
                    {
                        _state = State.GamePlaying;
                        _gamePlayTimer = _gamePlayTimerMax;
                        OnStateChanged?.Invoke(this, EventArgs.Empty);
                    }
                    break;
                }

            case State.GamePlaying:
                {
                    _gamePlayTimer -= Time.deltaTime;
                    if (_gamePlayTimer <= 0f)
                    {
                        _state = State.GameOver;
                        OnStateChanged?.Invoke(this, EventArgs.Empty);
                    }
                    break;
                }

            case State.GameOver:
                {
                    break;
                }
        }
    }
}
