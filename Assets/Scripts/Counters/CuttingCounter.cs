using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public event EventHandler OnCut;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    [SerializeField] private RecipeSO[] _recipesSO;

    private int _cuttingProgress;

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
            RecipeSO recipe = GetRecipeSOWithInput(parent.GetKitchenObject().GetKitchenObjectSO());
            if (parent.HasKitchenObject() && recipe != null)
            {
                // player carrying sth
                parent.GetKitchenObject().SetParent(this);
                UpdateProgress(0, recipe.cuttingProgressMax);
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
            }
        }
    }

    public override void InteractAlternate(IKitchenObjectParent parent)
    {
        if (HasKitchenObject())
        {
            RecipeSO recipe = GetRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
            if (recipe == null)
            {
                return;
            }
            UpdateProgress(_cuttingProgress + 1, recipe.cuttingProgressMax);

            if (_cuttingProgress != recipe.cuttingProgressMax)
            {
                return;
            }


            if (!parent.HasKitchenObject() && recipe.output != null)
            {
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(recipe.output, this);
            }

        }
    }

    private void UpdateProgress(int progress, int max)
    {
        _cuttingProgress = progress;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = (float)_cuttingProgress / max });
        if (_cuttingProgress > 0 && _cuttingProgress <= max)
        {
            OnCut?.Invoke(this, System.EventArgs.Empty);
        }
    }

    private KitchenObjectSO GetRecipeOutput(KitchenObjectSO inputKitchenObjectSO)
    {
        return GetRecipeSOWithInput(inputKitchenObjectSO)?.output;
    }

    private bool HasRecipeInput(KitchenObjectSO inputKitchenObjectSO)
    {
        RecipeSO recipe = GetRecipeSOWithInput(inputKitchenObjectSO);
        return recipe != null;
    }

    private RecipeSO GetRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (RecipeSO recipe in _recipesSO)
        {
            if (recipe.input == inputKitchenObjectSO)
            {
                return recipe;
            }
        }

        return null;
    }
}
