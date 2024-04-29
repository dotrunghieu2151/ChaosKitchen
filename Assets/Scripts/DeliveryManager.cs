using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DeliveryManager : NetworkBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeDelivered;
    public event EventHandler<OnRecipeDeliveryArgs> OnRecipeSuccess;
    public event EventHandler<OnRecipeDeliveryArgs> OnRecipeFailed;

    public class OnRecipeDeliveryArgs : EventArgs
    {
        public DeliveryCounter deliveryCounter;
    }

    public static DeliveryManager Instance { get; private set; }
    [SerializeField] private DeliveryRecipeListSO _deliveryListSO;
    private List<DeliveryRecipeSO> _waitingRecipeList;

    private float _timer = 4f;
    private float _timerMax = 4f;
    private int _waitingRecipeMaxCount = 5;
    private int _successfulRecipeCount = 0;

    private void Awake()
    {
        Instance = this;
        _waitingRecipeList = new List<DeliveryRecipeSO>();
    }

    private void Update()
    {
        if (!IsServer)
        {
            return;
        }
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            _timer = _timerMax;

            if (GameManager.Instance.IsGamePlaying() && _waitingRecipeList.Count < _waitingRecipeMaxCount)
            {
                int recipeIndex = UnityEngine.Random.Range(0, _deliveryListSO.recipeSOList.Count);
                SpawnNewWaitingRecipeClientRpc(recipeIndex);
            }
        }
    }

    [ClientRpc]
    private void SpawnNewWaitingRecipeClientRpc(int recipeIndex)
    {
        _waitingRecipeList.Add(_deliveryListSO.recipeSOList[recipeIndex]);

        OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
    }

    public void DeliverRecipe(PlateKitchenObject plate, DeliveryCounter deliveryCounter)
    {
        for (int i = 0; i < _waitingRecipeList.Count; ++i)
        {
            DeliveryRecipeSO waitingRecipe = _waitingRecipeList[i];

            if (waitingRecipe.kitchenObjectSOList.Count == plate.GetKitchenObjectSOList().Count)
            {
                // same number of Ingredients
                bool hasSameIngredients = true;
                foreach (KitchenObjectSO repiceKitchenObjectSO in waitingRecipe.kitchenObjectSOList)
                {
                    if (!plate.GetKitchenObjectSOList().Find(e => e == repiceKitchenObjectSO))
                    {
                        hasSameIngredients = false;
                        break;
                    }
                }
                if (hasSameIngredients)
                {
                    // all ingredients match
                    DeliverCorrectRecipeServerRpc(i, deliveryCounter);
                    return;
                }
            }
        }
        // delivery failed
        DeliverFailedRecipeServerRpc(deliveryCounter);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DeliverFailedRecipeServerRpc(NetworkBehaviourReference deliveryCounterRef)
    {
        DeliverFailedRecipeClientRpc(deliveryCounterRef);
    }

    [ClientRpc]
    private void DeliverFailedRecipeClientRpc(NetworkBehaviourReference deliveryCounterRef)
    {
        if (deliveryCounterRef.TryGet(out DeliveryCounter counter))
        {
            OnRecipeFailed?.Invoke(this, new OnRecipeDeliveryArgs { deliveryCounter = counter });
        };
    }

    [ServerRpc(RequireOwnership = false)]
    private void DeliverCorrectRecipeServerRpc(int recipeIndex, NetworkBehaviourReference deliveryCounterRef)
    {
        DeliverCorrectRecipeClientRpc(recipeIndex, deliveryCounterRef);
    }

    [ClientRpc]
    private void DeliverCorrectRecipeClientRpc(int recipeIndex, NetworkBehaviourReference deliveryCounterRef)
    {
        if (deliveryCounterRef.TryGet(out DeliveryCounter counter))
        {
            _waitingRecipeList.RemoveAt(recipeIndex);
            OnRecipeDelivered?.Invoke(this, EventArgs.Empty);
            OnRecipeSuccess?.Invoke(this, new OnRecipeDeliveryArgs { deliveryCounter = counter });
            ++_successfulRecipeCount;
        };
    }

    public List<DeliveryRecipeSO> GetWaitingRecipeSOList()
    {
        return _waitingRecipeList;
    }

    public int GetSuccessfulRecipeCount()
    {
        return _successfulRecipeCount;
    }
}
