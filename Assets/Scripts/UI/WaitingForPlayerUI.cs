using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingForPlayerUI : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.OnLocalPlayerReady += (sender, args) =>
        {
            if (GameManager.Instance.IsLocalPlayerReady())
            {
                Show();
            }
        };

        GameManager.Instance.OnStateChanged += (sender, args) =>
        {
            if (GameManager.Instance.IsCountdownToStart())
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
