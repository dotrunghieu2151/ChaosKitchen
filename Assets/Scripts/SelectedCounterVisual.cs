using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter _baseCounter;
    [SerializeField] private GameObject[] _visualGameObjects;
    // Start is called before the first frame update
    void Start()
    {
        PlayerMovement.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Player_OnSelectedCounterChanged(object sender, PlayerMovement.OnSelectedCounterChangeEventArgs e)
    {
        if (e.selectedCounter == _baseCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        foreach (GameObject visualGameObject in _visualGameObjects)
        {
            visualGameObject.SetActive(true);
        }
    }

    private void Hide()
    {
        foreach (GameObject visualGameObject in _visualGameObjects)
        {
            visualGameObject.SetActive(false);
        }
    }
}
