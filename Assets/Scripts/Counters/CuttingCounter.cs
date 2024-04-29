using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public static event EventHandler OnAnyCut;

    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }
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
            return !parent.HasKitchenObject() || parent.GetKitchenObject() is PlateKitchenObject;
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
                InteractLogicPlaceObjectOnCuttingCounterServerRpc();
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
                if (parent.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // player holding a plate, place onto the plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());
                    };
                }
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
            CutObjectServerRpc(parent.GetNetworkObject());
            TestCuttingProgressDoneServerRpc(parent.GetNetworkObject());
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void CutObjectServerRpc(NetworkObjectReference parentRef)
    {
        CutObjectClientRpc(parentRef);
    }

    [ClientRpc]
    private void CutObjectClientRpc(NetworkObjectReference parentRef)
    {
        RecipeSO recipe = GetRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
        UpdateProgress(_cuttingProgress + 1, recipe.cuttingProgressMax);
    }

    [ServerRpc(RequireOwnership = false)]
    private void TestCuttingProgressDoneServerRpc(NetworkObjectReference parentRef)
    {
        RecipeSO recipe = GetRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
        if (_cuttingProgress != recipe.cuttingProgressMax)
        {
            return;
        }

        if (parentRef.TryGet(out NetworkObject parentNetworkObject))
        {
            IKitchenObjectParent parent = parentNetworkObject.GetComponent<IKitchenObjectParent>();
            if (!parent.HasKitchenObject() && recipe.output != null)
            {
                KitchenObject.DestroyKitchenObject(GetKitchenObject());
                KitchenObject.SpawnKitchenObject(recipe.output, this);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicPlaceObjectOnCuttingCounterServerRpc()
    {
        InteractLogicPlaceObjectOnCuttingCounterClientRpc();
    }

    [ClientRpc]
    private void InteractLogicPlaceObjectOnCuttingCounterClientRpc()
    {
        _cuttingProgress = 0;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0f });
    }

    private void UpdateProgress(int progress, int max)
    {
        _cuttingProgress = progress;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = (float)_cuttingProgress / max });
        if (_cuttingProgress > 0 && _cuttingProgress <= max)
        {
            OnCut?.Invoke(this, System.EventArgs.Empty);
            OnAnyCut?.Invoke(this, System.EventArgs.Empty);
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
