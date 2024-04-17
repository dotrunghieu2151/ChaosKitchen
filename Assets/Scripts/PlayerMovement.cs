using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IKitchenObjectParent
{
    public static PlayerMovement Instance
    {
        get;
        private set;
    }
    public event EventHandler<OnSelectedCounterChangeEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangeEventArgs : EventArgs
    {
        public ClearCounter selectedCounter;
    }
    [SerializeField] private float _movementSpeed = 10f;
    [SerializeField] private GameInput _gameInput;

    [SerializeField] private float _playerRadius = 0.7f;
    [SerializeField] private float _playerHeight = 2f;
    [SerializeField] private float _interactDistance = 2f;
    [SerializeField] private LayerMask _countersLayerMask;
    [SerializeField] private Transform _kitchenObjectHoldPoint;

    private Vector3 _lastInteractDirection;

    private MovementState _movementState = MovementState.idle;
    private ClearCounter _selectedCounter;
    private KitchenObject _kitchenObject;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("THIS SHOUDL NOT HAPPEND ONlY ONE PLAYER");
        }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameInput.OnInteractAction += GameInput_OnInteractionAction;
    }

    private void GameInput_OnInteractionAction(object sender, System.EventArgs e)
    {
        if (_selectedCounter)
        {
            _selectedCounter.Interact(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleInteraction();
    }

    private void HandleInteraction()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();

        Vector3 transformVector3d = new(inputVector.x, 0f, inputVector.y);

        if (transformVector3d != Vector3.zero)
        {
            _lastInteractDirection = transformVector3d;
        }
        bool hit = Physics.Raycast(transform.position, _lastInteractDirection, out RaycastHit raycastHit, _interactDistance, _countersLayerMask);

        if (hit)
        {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                // has ClearCounter
                if (clearCounter != _selectedCounter)
                {
                    SetSelectedCounter(clearCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void HandleMovement()
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
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * _playerHeight, _playerRadius, transformVector, moveDistance);

        return canMove;
    }

    private void SetSelectedCounter(ClearCounter clearCounter)
    {
        _selectedCounter = clearCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangeEventArgs { selectedCounter = _selectedCounter });
    }

    public bool IsWalking()
    {
        return _movementState == MovementState.walking;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return _kitchenObject;
    }

    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return _kitchenObject != null;
    }

    public Transform GetChildAnchor()
    {
        return _kitchenObjectHoldPoint;
    }
}

public enum MovementState
{
    idle,
    walking,
}

