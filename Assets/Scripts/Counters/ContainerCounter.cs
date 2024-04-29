using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    public event EventHandler OnPlayerGrabbedObject;

    public override bool CanInteract(IKitchenObjectParent parent)
    {
        return !parent.HasKitchenObject();
    }


    public override void Interact(IKitchenObjectParent parent)
    {
        if (!parent.HasKitchenObject())
        {
            KitchenObject.SpawnKitchenObject(_kitchenObjectSO, parent);
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
        OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
    }
}
