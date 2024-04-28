using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
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

    private float _timer;
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
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            _timer = _timerMax;

            if (GameManager.Instance.IsGamePlaying() && _waitingRecipeList.Count < _waitingRecipeMaxCount)
            {
                DeliveryRecipeSO waitingRecipe = _deliveryListSO.recipeSOList[UnityEngine.Random.Range(0, _deliveryListSO.recipeSOList.Count)];
                _waitingRecipeList.Add(waitingRecipe);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
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
                    _waitingRecipeList.RemoveAt(i);
                    OnRecipeDelivered?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, new OnRecipeDeliveryArgs { deliveryCounter = deliveryCounter });
                    ++_successfulRecipeCount;
                    return;
                }
            }
        }
        // delivery failed
        OnRecipeFailed?.Invoke(this, new OnRecipeDeliveryArgs { deliveryCounter = deliveryCounter });
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
