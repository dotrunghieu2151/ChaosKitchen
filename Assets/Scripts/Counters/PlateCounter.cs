using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;
    [SerializeField] private float _plateSpawnInterval;
    [SerializeField] private float _platesSpawnMaxCount;

    private float _plateSpawnTimer;
    private int _platesSpawnCount;

    private void Update()
    {
        _plateSpawnTimer += Time.deltaTime;

        if (_plateSpawnTimer >= _plateSpawnInterval)
        {
            _plateSpawnTimer = 0f;
            if (_platesSpawnCount < _platesSpawnMaxCount)
            {
                // spawn visuals instead of plateSO
                OnPlateSpawned?.Invoke(this, System.EventArgs.Empty);
                ++_platesSpawnCount;
            }

        }
    }

    public override bool CanInteract(IKitchenObjectParent parent)
    {
        return !parent.HasKitchenObject() && HasPlates();
    }

    public override void Interact(IKitchenObjectParent parent)
    {
        if (!parent.HasKitchenObject())
        {
            if (HasPlates())
            {
                // give player plate
                --_platesSpawnCount;
                KitchenObject.SpawnKitchenObject(_kitchenObjectSO, parent);
                OnPlateRemoved?.Invoke(this, System.EventArgs.Empty);
            }
        }
    }

    private bool HasPlates()
    {
        return _platesSpawnCount > 0;
    }
}
