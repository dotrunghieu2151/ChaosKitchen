using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSOGameObjectList;
    private void Start()
    {
        foreach (KitchenObjectSO_GameObject e in kitchenObjectSOGameObjectList)
        {
            e.gameObject.SetActive(false);
        }

        plateKitchenObject.OnIngredientAdded += (sender, args) =>
        {
            foreach (KitchenObjectSO_GameObject e in kitchenObjectSOGameObjectList)
            {
                if (e.kitchenObjectSO == args.kitchenObjectSO)
                {
                    e.gameObject.SetActive(true);
                }
            }
        };
    }
}
