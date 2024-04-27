using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    private PlayerInputAction _playerInputAction;
    private void Awake()
    {
        Instance = this;
        _playerInputAction = new PlayerInputAction();
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
}
