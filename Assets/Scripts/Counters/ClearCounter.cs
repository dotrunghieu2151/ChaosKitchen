using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{

    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    public override bool CanInteract(IKitchenObjectParent parent)
    {
        return (!HasKitchenObject() && parent.HasKitchenObject()) ||
            (HasKitchenObject() && !parent.HasKitchenObject()) ||
            (
                HasKitchenObject() && parent.GetKitchenObject() is PlateKitchenObject
            ) ||
            GetKitchenObject() is PlateKitchenObject && parent.GetKitchenObject();
    }

    public override void Interact(IKitchenObjectParent parent)
    {
        if (!HasKitchenObject())
        {
            if (parent.HasKitchenObject())
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
                // player carrying sth, if it is a plate then
                if (parent.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // player holding a plate, place onto the plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    };
                }
                else if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                {
                    // counter has a plate
                    if (plateKitchenObject.TryAddIngredient(parent.GetKitchenObject().GetKitchenObjectSO()))
                    {
                        parent.GetKitchenObject().DestroySelf();
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
}
