using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnAnyObjectTrashed;

    new public static void ResetStaticData()
    {
        OnAnyObjectTrashed = null;
    }
    public override bool CanInteract(IKitchenObjectParent parent)
    {
        return parent.HasKitchenObject();
    }

    public override void Interact(IKitchenObjectParent parent)
    {
        if (parent.HasKitchenObject())
        {
            KitchenObject.DestroyKitchenObject(parent.GetKitchenObject());
            InteractLogicServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }

    [ClientRpc]
    private void InteractLogicClientRpc()
    {
        OnAnyObjectTrashed?.Invoke(this, System.EventArgs.Empty);
    }


}
