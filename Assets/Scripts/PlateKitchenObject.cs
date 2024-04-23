using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;

    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }
    [SerializeField] private List<KitchenObjectSO> _validKitchenObjectSO;
    private List<KitchenObjectSO> _kitchenObjectSOList;

    private void Awake()
    {
        _kitchenObjectSOList = new List<KitchenObjectSO>();
    }
    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return _kitchenObjectSOList;
    }
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!_validKitchenObjectSO.Contains(kitchenObjectSO))
        {
            return false;
        }

        // prevent duplicates
        if (_kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        }
        _kitchenObjectSOList.Add(kitchenObjectSO);
        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs { kitchenObjectSO = kitchenObjectSO });

        return true;
    }
}
