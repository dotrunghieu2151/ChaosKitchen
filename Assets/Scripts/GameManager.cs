using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{

    public event EventHandler OnLocalPlayerReady;
    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    public static GameManager Instance { get; private set; }
    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    private NetworkVariable<State> _state = new NetworkVariable<State>(State.WaitingToStart);
    private bool _isLocalPlayerReady;

    private Dictionary<ulong, bool> _playerReadyDict;

    [SerializeField] private NetworkVariable<float> _countdownToStartTimer = new NetworkVariable<float>(3f);

    private NetworkVariable<float> _gamePlayTimer = new NetworkVariable<float>(300f);
    [SerializeField] private float _gamePlayTimerMax = 300f;
    [SerializeField] private CinemachineVirtualCamera _vcam;

    private bool _isGamePause = false;

    public void SetCameraPlayerTarget(PlayerMovement player)
    {
        _vcam.Follow = player.gameObject.transform;
    }

    public bool IsGamePlaying()
    {
        return _state.Value == State.GamePlaying;
    }

    public bool IsCountdownToStart()
    {
        return _state.Value == State.CountdownToStart;
    }

    public bool IsGameOver()
    {
        return _state.Value == State.GameOver;
    }

    public bool IsGamePaused()
    {
        return _isGamePause;
    }

    public float GetCountdownToStartTimer()
    {
        return _countdownToStartTimer.Value;
    }

    public bool IsLocalPlayerReady()
    {
        return _isLocalPlayerReady;
    }

    public float GetGamePlayingTimerNormalize()
    {
        return 1 - (_gamePlayTimer.Value / _gamePlayTimerMax);
    }

    private void Awake()
    {
        Instance = this;
        _playerReadyDict = new Dictionary<ulong, bool>();
    }
    // Start is called before the first frame update
    void Start()
    {
        GameInput.Instance.OnPauseAction += (sender, args) =>
        {
            TogglePauseGame();
        };

        GameInput.Instance.OnInteractAction += (sender, args) =>
        {
            if (_state.Value == State.WaitingToStart)
            {
                _isLocalPlayerReady = true;
                OnLocalPlayerReady?.Invoke(this, EventArgs.Empty);
                SetPlayerReadyServerRpc();
            }
        };

    }

    public override void OnNetworkSpawn()
    {
        _state.OnValueChanged += (State prev, State newVal) =>
        {
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        };
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        _playerReadyDict[serverRpcParams.Receive.SenderClientId] = true;

        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!_playerReadyDict.ContainsKey(clientId) || !_playerReadyDict[clientId])
            {
                // not all players are ready
                return;
            }
        }

        // all players are ready
        _state.Value = State.CountdownToStart;
    }


    // Update is called once per frame
    void Update()
    {
        if (!IsServer)
        {
            return;
        }
        switch (_state.Value)
        {
            case State.WaitingToStart:
                {
                    break;
                }

            case State.CountdownToStart:
                {
                    _countdownToStartTimer.Value -= Time.deltaTime;
                    if (_countdownToStartTimer.Value <= 0f)
                    {
                        _state.Value = State.GamePlaying;
                        _gamePlayTimer.Value = _gamePlayTimerMax;

                    }
                    break;
                }

            case State.GamePlaying:
                {
                    _gamePlayTimer.Value -= Time.deltaTime;
                    if (_gamePlayTimer.Value <= 0f)
                    {
                        _state.Value = State.GameOver;
                    }
                    break;
                }

            case State.GameOver:
                {
                    break;
                }
        }
    }

    public void TogglePauseGame()
    {
        _isGamePause = !_isGamePause;
        if (_isGamePause)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }
}
