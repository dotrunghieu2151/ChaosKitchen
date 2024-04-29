using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class KitchenObject : NetworkBehaviour
{
    public static void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent parent)
    {
        KitchenGameMultiplayer.Instance.SpawnKitchenObject(kitchenObjectSO, parent);
    }

    public static void DestroyKitchenObject(KitchenObject kitchenObject)
    {
        KitchenGameMultiplayer.Instance.DestroyKitchenObject(kitchenObject);
    }

    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    private IKitchenObjectParent _parent;
    private FollowTransform _followTransform;

    protected virtual void Awake()
    {
        _followTransform = GetComponent<FollowTransform>();
    }

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return _kitchenObjectSO;
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }

        plateKitchenObject = null;
        return false;
    }

    public void SetParent(IKitchenObjectParent parent)
    {
        SetKitchenObjectParentServerRpc(parent.GetNetworkObject());
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetKitchenObjectParentServerRpc(NetworkObjectReference parentRef)
    {
        SetKitchenObjectParentClientRpc(parentRef);
    }

    [ClientRpc]
    private void SetKitchenObjectParentClientRpc(NetworkObjectReference parentRef)
    {
        if (parentRef.TryGet(out NetworkObject parentNetworkObject))
        {
            IKitchenObjectParent parent = parentNetworkObject.GetComponent<IKitchenObjectParent>();
            if (_parent != null)
            {
                _parent.ClearKitchenObject();
            }

            _parent = parent;
            if (_parent.HasKitchenObject())
            {
                Debug.LogError("COUNTER ALREADY HAS A KITCHEN OBJECT");
            }
            _parent.SetKitchenObject(this);

            _followTransform.SetTargetTransform(_parent.GetChildAnchor().transform);
        }
    }


    public IKitchenObjectParent GetParent()
    {
        return _parent;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void ClearKitchenObjectOnParent()
    {
        _parent.ClearKitchenObject();
    }
}
