using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _recipeDeliveredText;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnStateChanged += (sender, args) =>
        {
            if (GameManager.Instance.IsGameOver())
            {
                _recipeDeliveredText.text = DeliveryManager.Instance.GetSuccessfulRecipeCount().ToString();
                Show();
            }
            else
            {
                Hide();
            }
        };
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
