using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private Transform _recipeTemplate;

    private void Awake()
    {
        _recipeTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeDelivered += (sender, args) =>
        {
            UpdateVisual();
        };

        DeliveryManager.Instance.OnRecipeSpawned += (sender, args) =>
        {
            UpdateVisual();
        };

        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child in _container)
        {
            if (child == _recipeTemplate)
            {
                continue;
            }

            Destroy(child.gameObject);
        }

        foreach (DeliveryRecipeSO recipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList())
        {
            Transform recipeTransform = Instantiate(_recipeTemplate, _container);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);
        }
    }
}
