using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent parent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, parent.GetChildAnchor());

        KitchenObject obj = kitchenObjectTransform.GetComponent<KitchenObject>();
        obj.SetParent(parent);

        return obj;
    }

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

    public void DestroySelf()
    {
        _parent.ClearKitchenObject();

        Destroy(gameObject);
    }
}
