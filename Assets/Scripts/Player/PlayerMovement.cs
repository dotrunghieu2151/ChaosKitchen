using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnPlayerPickup;

    public static void ResetStaticData()
    {
        OnPlayerPickup = null;
    }
    public static PlayerMovement Instance
    {
        get;
        private set;
    }
    public event EventHandler<OnSelectedCounterChangeEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangeEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }
    [SerializeField] private float _movementSpeed = 10f;
    [SerializeField] private float _playerRadius = 0.2f;
    [SerializeField] private float _playerHeight = 2f;
    [SerializeField] private float _interactDistance = 2f;
    [SerializeField] private LayerMask _countersLayerMask;
    [SerializeField] private Transform _kitchenObjectHoldPoint;

    private Vector3 _lastInteractDirection;

    private MovementState _movementState = MovementState.idle;
    private BaseCounter _selectedCounter;
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
        GameInput.Instance.OnInteractAction += GameInput_OnInteractionAction;
        GameInput.Instance.OnInteractAlternateAction += GameInput_OnInteractionAlternateAction;
    }

    private void GameInput_OnInteractionAction(object sender, System.EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying())
        {
            return;
        }
        if (_selectedCounter)
        {
            _selectedCounter.Interact(this);
        }
    }

    private void GameInput_OnInteractionAlternateAction(object sender, System.EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying())
        {
            return;
        }
        if (_selectedCounter)
        {
            _selectedCounter.InteractAlternate(this);
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
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        Vector3 transformVector3d = new(inputVector.x, 0f, inputVector.y);

        if (transformVector3d != Vector3.zero)
        {
            _lastInteractDirection = transformVector3d;
        }
        bool hit = Physics.Raycast(transform.position, _lastInteractDirection, out RaycastHit raycastHit, _interactDistance, _countersLayerMask);

        if (hit)
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                // has ClearCounter
                if (baseCounter != _selectedCounter && baseCounter.CanInteract(this))
                {
                    SetSelectedCounter(baseCounter);
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
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        Vector3 transformVector3d = new(inputVector.x, 0f, inputVector.y);

        float moveDistance = _movementSpeed * Time.deltaTime;

        bool canMove = CanMove(transformVector3d, moveDistance);

        if (!canMove)
        {
            // check if can move X
            Vector3 moveX = new Vector3(transformVector3d.x, 0f, 0f).normalized;
            canMove = (moveX.x < -0.5f || moveX.x > 0.5f) && CanMove(moveX, moveDistance);
            if (canMove)
            {
                transformVector3d = moveX;
            }
            else
            {
                // check if can move Z
                Vector3 moveZ = new Vector3(0f, 0f, transformVector3d.z).normalized;
                canMove = (moveX.z < -0.5f || moveX.z > 0.5f) && CanMove(moveZ, moveDistance);
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

    private void SetSelectedCounter(BaseCounter baseCounter)
    {
        _selectedCounter = baseCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangeEventArgs { selectedCounter = _selectedCounter });
    }

    public bool IsWalking()
    {
        return _movementState == MovementState.walking;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;
        if (_kitchenObject != null)
        {
            OnPlayerPickup?.Invoke(this, EventArgs.Empty);
        }
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

