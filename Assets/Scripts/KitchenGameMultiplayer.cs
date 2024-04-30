using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenGameMultiplayer : NetworkBehaviour
{
    [SerializeField] private KitchenObjectSOList _kitchenObjectSOList;
    public static KitchenGameMultiplayer Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent parent)
    {
        SpawnKitchenObjectServerRpc(GetKitchenObjectSOIndex(kitchenObjectSO), parent.GetNetworkObject());
    }

    public void DestroyKitchenObject(KitchenObject kitchenObject)
    {
        DestroyKitchenObjectServerRpc(kitchenObject.NetworkObject);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnKitchenObjectServerRpc(int kitchenObjectSOIndex, NetworkObjectReference parentRef)
    {
        KitchenObjectSO kitchenObjectSO = GetKitchenObjectSO(kitchenObjectSOIndex);
        if (parentRef.TryGet(out NetworkObject parentNetworkObject))
        {
            IKitchenObjectParent parent = parentNetworkObject.GetComponent<IKitchenObjectParent>();
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, parent.GetChildAnchor());
            NetworkObject kitchenNetworkObject = kitchenObjectTransform.GetComponent<NetworkObject>();
            kitchenNetworkObject.Spawn(true);
            KitchenObject obj = kitchenObjectTransform.GetComponent<KitchenObject>();

            obj.SetParent(parent);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyKitchenObjectServerRpc(NetworkObjectReference kitchenObjectRef)
    {
        if (kitchenObjectRef.TryGet(out NetworkObject kitchenObjectNetworkObject))
        {
            KitchenObject kitchenObject = kitchenObjectNetworkObject.GetComponent<KitchenObject>();
            ClearKitchenObjectOnParentClientRpc(kitchenObjectRef);
            kitchenObject.DestroySelf();
        }
    }

    [ClientRpc]
    private void ClearKitchenObjectOnParentClientRpc(NetworkObjectReference kitchenObjectRef)
    {
        if (kitchenObjectRef.TryGet(out NetworkObject kitchenObjectNetworkObject))
        {
            KitchenObject kitchenObject = kitchenObjectNetworkObject.GetComponent<KitchenObject>();
            kitchenObject.ClearKitchenObjectOnParent();
        }
    }

    public int GetKitchenObjectSOIndex(KitchenObjectSO kitchenObjectSO)
    {
        return _kitchenObjectSOList.kitchenObjectSOList.IndexOf(kitchenObjectSO);
    }

    public KitchenObjectSO GetKitchenObjectSO(int index)
    {
        return _kitchenObjectSOList.kitchenObjectSOList[index];
    }
}
