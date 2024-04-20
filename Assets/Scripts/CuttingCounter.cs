using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private RecipeSO[] _recipesSO;

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
            if (parent.HasKitchenObject() && HasRecipeInput(parent.GetKitchenObject().GetKitchenObjectSO()))
            {
                // player carrying sth
                parent.GetKitchenObject().SetParent(this);
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
            KitchenObjectSO recipeOutput = GetRecipeOutput(GetKitchenObject().GetKitchenObjectSO());
            if (!parent.HasKitchenObject() && recipeOutput != null)
            {
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(recipeOutput, this);
            }
        }
    }

    private KitchenObjectSO GetRecipeOutput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (RecipeSO recipe in _recipesSO)
        {
            if (recipe.input == inputKitchenObjectSO)
            {
                return recipe.output;
            }
        }
        return null;
    }

    private bool HasRecipeInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (RecipeSO recipe in _recipesSO)
        {
            if (recipe.input == inputKitchenObjectSO)
            {
                return true;
            }
        }

        return false;
    }


}
