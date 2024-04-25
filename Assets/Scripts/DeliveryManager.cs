using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeDelivered;

    public static DeliveryManager Instance { get; private set; }
    [SerializeField] private DeliveryRecipeListSO _deliveryListSO;
    private List<DeliveryRecipeSO> _waitingRecipeList;

    private float _timer;
    private float _timerMax = 4f;
    private int _waitingRecipeMaxCount = 5;

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

            if (_waitingRecipeList.Count < _waitingRecipeMaxCount)
            {
                DeliveryRecipeSO waitingRecipe = _deliveryListSO.recipeSOList[UnityEngine.Random.Range(0, _deliveryListSO.recipeSOList.Count)];
                _waitingRecipeList.Add(waitingRecipe);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plate)
    {
        for (int i = 0; i < _waitingRecipeList.Count; ++i)
        {
            DeliveryRecipeSO waitingRecipe = _waitingRecipeList[i];

            if (waitingRecipe.kitchenObjectSOList.Count == plate.GetKitchenObjectSOList().Count)
            {
                // same number of Ingredients
                foreach (KitchenObjectSO repiceKitchenObjectSO in waitingRecipe.kitchenObjectSOList)
                {
                    if (!plate.GetKitchenObjectSOList().Find(e => e == repiceKitchenObjectSO))
                    {
                        return;
                    }
                }

                // all ingredients match
                _waitingRecipeList.RemoveAt(i);
                OnRecipeDelivered?.Invoke(this, EventArgs.Empty);
                return;
            }
        }
    }

    public List<DeliveryRecipeSO> GetWaitingRecipeSOList()
    {
        return _waitingRecipeList;
    }
}
