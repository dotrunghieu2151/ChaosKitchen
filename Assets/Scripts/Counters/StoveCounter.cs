using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Unity.Netcode;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{

    [SerializeField] private FryingRecipeSO[] _fryingRecipeSOs;
    [SerializeField] private BurningRecipeSO[] _burningRecipeSOs;

    public event EventHandler<OnStateChangeEventArgs> OnStateChanged;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public class OnStateChangeEventArgs
    {
        public State state;
    }

    private NetworkVariable<float> _fryingTimer = new NetworkVariable<float>(0f);
    private NetworkVariable<float> _burningTimer = new NetworkVariable<float>(0f);
    private FryingRecipeSO _currentFryingRecipe;
    private BurningRecipeSO _currentBurningRecipe;
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burnt
    }

    private NetworkVariable<State> _currentState = new NetworkVariable<State>(State.Idle);

    public override void OnNetworkSpawn()
    {
        _fryingTimer.OnValueChanged += (float previousValue, float newValue) =>
        {
            float fryingTimerMax = _currentFryingRecipe != null ? _currentFryingRecipe.fryingTimerMax : 1f;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = newValue / fryingTimerMax });
        };

        _burningTimer.OnValueChanged += (float previousValue, float newValue) =>
        {
            float burningTimerMax = _currentBurningRecipe != null ? _currentBurningRecipe.burningTimerMax : 1f;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = newValue / burningTimerMax });
        };

        _currentState.OnValueChanged += (State previousValue, State newValue) =>
        {
            OnStateChanged?.Invoke(this, new OnStateChangeEventArgs { state = newValue });
            if (newValue == State.Burnt || newValue == State.Idle)
            {
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0f });
            }
        };
    }

    private void Update()
    {
        if (!IsServer)
        {
            return;
        }
        if (HasKitchenObject())
        {
            switch (_currentState.Value)
            {
                case State.Idle:
                    {
                        break;
                    }

                case State.Frying:
                    {
                        _fryingTimer.Value += Time.deltaTime;

                        if (_fryingTimer.Value > _currentFryingRecipe.fryingTimerMax)
                        {
                            // fried
                            KitchenObject.DestroyKitchenObject(GetKitchenObject());
                            KitchenObject.SpawnKitchenObject(_currentFryingRecipe.output, this);
                            _currentState.Value = State.Fried;
                            _fryingTimer.Value = 0f;
                            _burningTimer.Value = 0f;
                        }
                        break;
                    }

                case State.Fried:
                    {
                        _burningTimer.Value += Time.deltaTime;
                        if (_burningTimer.Value > _currentBurningRecipe.burningTimerMax)
                        {
                            // fried
                            KitchenObject.DestroyKitchenObject(GetKitchenObject());
                            KitchenObject.SpawnKitchenObject(_currentBurningRecipe.output, this);
                            _currentState.Value = State.Burnt;
                        }
                        break;
                    }

                case State.Burnt:
                    {
                        break;
                    }
            }
        }
    }

    public override bool CanInteract(IKitchenObjectParent parent)
    {
        if (!HasKitchenObject())
        {
            return parent.HasKitchenObject() && HasRecipeInput(parent.GetKitchenObject().GetKitchenObjectSO());
        }
        else
        {
            return !parent.HasKitchenObject() || parent.GetKitchenObject() is PlateKitchenObject;
        }
    }

    public override void Interact(IKitchenObjectParent parent)
    {
        if (!HasKitchenObject())
        {
            FryingRecipeSO recipe = GetRecipeSOWithInput(parent.GetKitchenObject().GetKitchenObjectSO());
            if (parent.HasKitchenObject() && recipe != null)
            {
                // player carrying sth
                int kitchenObjectSOIndex = KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(parent.GetKitchenObject().GetKitchenObjectSO());
                parent.GetKitchenObject().SetParent(this);
                InteractLogicPlaceObjectOnCounterServerRpc(kitchenObjectSOIndex);
            }
            else
            {
                //player has nothing
            }
        }
        else
        {
            if (parent.HasKitchenObject())
            {
                if (parent.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // player holding a plate, place onto the plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());
                        SetStateIdleServerRpc();
                    };
                }
            }
            else
            {
                // give to the player
                GetKitchenObject().SetParent(parent);
                SetStateIdleServerRpc();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetStateIdleServerRpc()
    {
        _currentState.Value = State.Idle;
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicPlaceObjectOnCounterServerRpc(int kitchenObjectSOIndex)
    {
        _fryingTimer.Value = 0f;
        _currentState.Value = State.Frying;
        SetFryingRecipeSOClientRpc(kitchenObjectSOIndex);
        SetBurningRecipeSOClientRpc(kitchenObjectSOIndex);
    }

    [ClientRpc]
    private void SetFryingRecipeSOClientRpc(int KitchenObjectSOIndex)
    {
        FryingRecipeSO recipe = GetRecipeSOWithInput(KitchenGameMultiplayer.Instance.GetKitchenObjectSO(KitchenObjectSOIndex));
        _currentFryingRecipe = recipe;
    }


    [ClientRpc]
    private void SetBurningRecipeSOClientRpc(int KitchenObjectSOIndex)
    {
        FryingRecipeSO recipe = GetRecipeSOWithInput(KitchenGameMultiplayer.Instance.GetKitchenObjectSO(KitchenObjectSOIndex));
        _currentBurningRecipe = GetBurningRecipeSOWithInput(recipe.output);
    }

    private KitchenObjectSO GetRecipeOutput(KitchenObjectSO inputKitchenObjectSO)
    {
        return GetRecipeSOWithInput(inputKitchenObjectSO)?.output;
    }

    private bool HasRecipeInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO recipe = GetRecipeSOWithInput(inputKitchenObjectSO);
        return recipe != null;
    }

    private FryingRecipeSO GetRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO recipe in _fryingRecipeSOs)
        {
            if (recipe.input == inputKitchenObjectSO)
            {
                return recipe;
            }
        }

        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO recipe in _burningRecipeSOs)
        {
            if (recipe.input == inputKitchenObjectSO)
            {
                return recipe;
            }
        }

        return null;
    }

    public bool IsFried()
    {
        return _currentState.Value == State.Fried;
    }
}
