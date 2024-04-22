using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{

    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    public override bool CanInteract(IKitchenObjectParent parent)
    {
        return (!HasKitchenObject() && parent.HasKitchenObject()) ||
            (HasKitchenObject() && !parent.HasKitchenObject());
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
                // player
            }
            else
            {
                // give to the player
                GetKitchenObject().SetParent(parent);
            }
        }
    }
}
