using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 10f;
    [SerializeField] private GameInput _gameInput;

    private MovementState _movementState = MovementState.idle;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();

        Vector3 transformVector3d = new(inputVector.x, 0f, inputVector.y);

        float moveDistance = _movementSpeed * Time.deltaTime;

        bool canMove = CanMove(transformVector3d, moveDistance);

        if (!canMove)
        {
            // check if can move X
            Vector3 moveX = new Vector3(transformVector3d.x, 0f, 0f).normalized;
            canMove = CanMove(moveX, moveDistance);
            if (canMove)
            {
                transformVector3d = moveX;
            }
            else
            {
                // check if can move Z
                Vector3 moveZ = new Vector3(0f, 0f, transformVector3d.z).normalized;
                canMove = CanMove(moveZ, moveDistance);
                if (canMove)
                {
                    transformVector3d = moveZ;
                }
            }
        }

        if (canMove)
        {
            transform.position += transformVector3d * moveDistance;
        }

        _movementState = transformVector3d == Vector3.zero ? MovementState.idle : MovementState.walking;

        float rotationSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, transformVector3d, rotationSpeed * Time.deltaTime);
    }

    private bool CanMove(Vector3 transformVector, float moveDistance)
    {
        float playerRadius = 0.7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, transformVector, moveDistance);

        return canMove;
    }

    public bool IsWalking()
    {
        return _movementState == MovementState.walking;
    }
}

public enum MovementState
{
    idle,
    walking,
}

