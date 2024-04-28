using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnRebindBinding;
    public enum Binding
    {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Interact,
        InteractAlternate,
        Pause
    }
    public static GameInput Instance { get; private set; }
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    private PlayerInputAction _playerInputAction;

    private const string PLAYER_PREFS_INPUT_BINDINGS = "PlayerPrefsInputBindings";

    private void Awake()
    {
        Instance = this;
        _playerInputAction = new PlayerInputAction();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_INPUT_BINDINGS))
        {
            _playerInputAction.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_INPUT_BINDINGS));
        }

        _playerInputAction.Player.Enable();
        _playerInputAction.Player.Interact.performed += Interact_perform;
        _playerInputAction.Player.InteractAlternate.performed += Interact_performAlternate;
        _playerInputAction.Player.Pause.performed += Pause_perform;
    }

    private void OnDestroy()
    {
        _playerInputAction.Player.Interact.performed -= Interact_perform;
        _playerInputAction.Player.InteractAlternate.performed -= Interact_performAlternate;
        _playerInputAction.Player.Pause.performed -= Pause_perform;

        _playerInputAction.Dispose();
    }

    private void Pause_perform(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, System.EventArgs.Empty);
    }

    private void Interact_perform(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performAlternate(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }
    // Start is called before the first frame update

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = _playerInputAction.Player.Move.ReadValue<Vector2>();

        return inputVector.normalized;
    }

    public string GetBindingText(Binding binding)
    {
        string input;
        switch (binding)
        {
            default:
            case Binding.MoveUp:
                {
                    input = _playerInputAction.Player.Move.bindings[1].ToDisplayString();
                    break;
                }

            case Binding.MoveDown:
                {
                    input = _playerInputAction.Player.Move.bindings[2].ToDisplayString();
                    break;
                }

            case Binding.MoveLeft:
                {
                    input = _playerInputAction.Player.Move.bindings[3].ToDisplayString();
                    break;
                }

            case Binding.MoveRight:
                {
                    input = _playerInputAction.Player.Move.bindings[4].ToDisplayString();
                    break;
                }

            case Binding.Interact:
                {
                    input = _playerInputAction.Player.Interact.bindings[0].ToDisplayString();
                    break;
                }

            case Binding.InteractAlternate:
                {
                    input = _playerInputAction.Player.InteractAlternate.bindings[0].ToDisplayString();
                    break;
                }

            case Binding.Pause:
                {
                    input = _playerInputAction.Player.Pause.bindings[0].ToDisplayString();
                    break;
                }
        }

        return input;
    }


    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        _playerInputAction.Player.Disable();
        InputAction inputAction;
        int bindingIndex;
        switch (binding)
        {
            default:
            case Binding.MoveUp:
                {
                    inputAction = _playerInputAction.Player.Move;
                    bindingIndex = 1;
                    break;
                }
            case Binding.MoveDown:
                {
                    inputAction = _playerInputAction.Player.Move;
                    bindingIndex = 2;
                    break;
                }
            case Binding.MoveLeft:
                {
                    inputAction = _playerInputAction.Player.Move;
                    bindingIndex = 3;
                    break;
                }
            case Binding.MoveRight:
                {
                    inputAction = _playerInputAction.Player.Move;
                    bindingIndex = 4;
                    break;
                }
            case Binding.Interact:
                {
                    inputAction = _playerInputAction.Player.Interact;
                    bindingIndex = 0;
                    break;
                }
            case Binding.InteractAlternate:
                {
                    inputAction = _playerInputAction.Player.InteractAlternate;
                    bindingIndex = 0;
                    break;
                }
            case Binding.Pause:
                {
                    inputAction = _playerInputAction.Player.Pause;
                    bindingIndex = 0;
                    break;
                }
        }

        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback =>
            {
                _playerInputAction.Player.Enable();
                onActionRebound();

                PlayerPrefs.SetString(PLAYER_PREFS_INPUT_BINDINGS, _playerInputAction.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

                OnRebindBinding?.Invoke(this, EventArgs.Empty);
            })
            .Start();
    }
}
