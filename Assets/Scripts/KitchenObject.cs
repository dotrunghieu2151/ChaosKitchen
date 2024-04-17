using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    private IKitchenObjectParent _parent;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return _kitchenObjectSO;
    }

    public void SetParent(IKitchenObjectParent parent)
    {
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

        transform.parent = _parent.GetChildAnchor();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetParent()
    {
        return _parent;
    }
}
