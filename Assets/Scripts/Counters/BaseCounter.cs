using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BaseCounter : NetworkBehaviour, IKitchenObjectParent
{
  public static event EventHandler OnAnyObjectPlacedHere;

  public static void ResetStaticData()
  {
    OnAnyObjectPlacedHere = null;
  }
  [SerializeField] private Transform _topPoint;

  private KitchenObject _kitchenObject;



  public virtual void Interact(IKitchenObjectParent parent)
  {
  }

  public virtual void InteractAlternate(IKitchenObjectParent parent)
  {
  }

  public virtual bool CanInteract(IKitchenObjectParent parent)
  {
    return true;
  }

  public Transform GetTopPoint()
  {
    return _topPoint;
  }

  public void SetKitchenObject(KitchenObject kitchenObject)
  {
    _kitchenObject = kitchenObject;
    if (_kitchenObject != null)
    {
      OnAnyObjectPlacedHere?.Invoke(this, System.EventArgs.Empty);
    }
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

  public NetworkObject GetNetworkObject()
  {
    return NetworkObject;
  }
}
