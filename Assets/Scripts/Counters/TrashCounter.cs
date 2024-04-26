using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnAnyObjectTrashed;
    public override bool CanInteract(IKitchenObjectParent parent)
    {
        return parent.HasKitchenObject();
    }

    public override void Interact(IKitchenObjectParent parent)
    {
        if (parent.HasKitchenObject())
        {
            parent.GetKitchenObject().DestroySelf();
            OnAnyObjectTrashed?.Invoke(this, System.EventArgs.Empty);
        }
    }
}
