using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter
{

    [SerializeField] private FryingRecipeSO[] _fryingRecipeSOs;

    public override bool CanInteract(IKitchenObjectParent parent)
    {
        return (!HasKitchenObject() && parent.HasKitchenObject()) ||
            (HasKitchenObject() && !parent.HasKitchenObject());
    }

    public override void Interact(IKitchenObjectParent parent)
    {

    }
}
