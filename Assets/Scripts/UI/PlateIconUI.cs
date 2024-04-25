using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject _plate;
    [SerializeField] private Transform _iconTemplate;

    private void Awake()
    {
        _iconTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        _plate.OnIngredientAdded += (sender, args) =>
        {
            UpdateVisual();
        };
    }

    private void UpdateVisual()
    {
        foreach (Transform child in transform)
        {
            if (child == _iconTemplate)
            {
                continue;
            }
            Destroy(child.gameObject);
        }
        foreach (KitchenObjectSO obj in _plate.GetKitchenObjectSOList())
        {
            // use transofrm to become child of this obj
            Transform iconTransform = Instantiate(_iconTemplate, transform);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<PlateIconSingleUI>().SetKitchenObjectSO(obj);
        }
    }
}
