using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform _topPoint;
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    private KitchenObject _kitchenObject;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Interact(IKitchenObjectParent parent)
    {
        if (!_kitchenObject)
        {
            Transform kitchenObjectTransform = Instantiate(_kitchenObjectSO.prefab, _topPoint);

            kitchenObjectTransform.GetComponent<KitchenObject>().SetParent(this);
        }
        else
        {
            // give object to the player
            _kitchenObject.GetComponent<KitchenObject>().SetParent(parent);
        }
    }

    public Transform GetTopPoint()
    {
        return _topPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return _kitchenObject;
    }

    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return _kitchenObject != null;
    }

    public Transform GetChildAnchor()
    {
        return GetTopPoint();
    }
}
