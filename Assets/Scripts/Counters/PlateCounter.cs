using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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
        if (!IsServer)
        {
            return;
        }
        _plateSpawnTimer += Time.deltaTime;

        if (GameManager.Instance.IsGamePlaying() && _plateSpawnTimer >= _plateSpawnInterval)
        {
            _plateSpawnTimer = 0f;
            if (_platesSpawnCount < _platesSpawnMaxCount)
            {
                SpawnPlatesServerRpc();
            }

        }
    }

    [ServerRpc]
    private void SpawnPlatesServerRpc()
    {
        SpawnPlatesClientRpc();
    }

    [ClientRpc]
    private void SpawnPlatesClientRpc()
    {
        // spawn visuals instead of plateSO
        OnPlateSpawned?.Invoke(this, System.EventArgs.Empty);
        ++_platesSpawnCount;
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

                KitchenObject.SpawnKitchenObject(_kitchenObjectSO, parent);
                InteractLogicServerRpc();
            }
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
        --_platesSpawnCount;
        OnPlateRemoved?.Invoke(this, EventArgs.Empty);
    }
    private bool HasPlates()
    {
        return _platesSpawnCount > 0;
    }
}
