using System;
using System.Collections;
using System.Collections.Generic;
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

    private float _fryingTimer;
    private float _burningTimer;
    private FryingRecipeSO _currentFryingRecipe;
    private BurningRecipeSO _currentBurningRecipe;
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burnt
    }

    private State _currentState;


    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (_currentState)
            {
                case State.Idle:
                    {
                        break;
                    }

                case State.Frying:
                    {
                        _fryingTimer += Time.deltaTime;
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = _fryingTimer / _currentFryingRecipe.fryingTimerMax });
                        if (_fryingTimer > _currentFryingRecipe.fryingTimerMax)
                        {
                            // fried
                            GetKitchenObject().DestroySelf();
                            KitchenObject.SpawnKitchenObject(_currentFryingRecipe.output, this);
                            _currentState = State.Fried;
                            OnStateChanged?.Invoke(this, new OnStateChangeEventArgs { state = _currentState });
                            _burningTimer = 0f;
                            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = _burningTimer });
                        }
                        break;
                    }

                case State.Fried:
                    {
                        _burningTimer += Time.deltaTime;
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = _burningTimer / _currentBurningRecipe.burningTimerMax });
                        if (_burningTimer > _currentBurningRecipe.burningTimerMax)
                        {
                            // fried
                            GetKitchenObject().DestroySelf();
                            KitchenObject.SpawnKitchenObject(_currentBurningRecipe.output, this);
                            _currentState = State.Burnt;
                            OnStateChanged?.Invoke(this, new OnStateChangeEventArgs { state = _currentState });
                        }
                        break;
                    }

                case State.Burnt:
                    {
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0f });
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
            return !parent.HasKitchenObject();
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
                parent.GetKitchenObject().SetParent(this);
                _currentFryingRecipe = recipe;
                _currentBurningRecipe = GetBurningRecipeSOWithInput(recipe.output);
                _currentState = State.Frying;
                OnStateChanged?.Invoke(this, new OnStateChangeEventArgs { state = _currentState });
                _fryingTimer = 0f;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = _fryingTimer });
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
                // player
            }
            else
            {
                // give to the player
                GetKitchenObject().SetParent(parent);
                _currentFryingRecipe = null;
                _currentBurningRecipe = null;
                _currentState = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangeEventArgs { state = _currentState });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0f });
            }
        }
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
}
