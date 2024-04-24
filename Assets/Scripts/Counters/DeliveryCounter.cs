using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{

    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    public override bool CanInteract(IKitchenObjectParent parent)
    {
        return parent.GetKitchenObject() is PlateKitchenObject;

    }

    public override void Interact(IKitchenObjectParent parent)
    {
        if (parent.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
        {
            DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
            plateKitchenObject.DestroySelf();
        }
        else
        {

        }
    }
}
