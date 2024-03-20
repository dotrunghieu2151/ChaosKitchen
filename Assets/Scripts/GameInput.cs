using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputAction _playerInputAction;
    private void Awake()
    {
        _playerInputAction = new PlayerInputAction();
        _playerInputAction.Player.Enable();
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
