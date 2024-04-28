using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurntWarningUI : MonoBehaviour
{
    [SerializeField] private StoveCounter _stoveCounter;
    // Start is called before the first frame update
    void Start()
    {
        _stoveCounter.OnProgressChanged += (sender, args) =>
        {
            float burnShowAmount = 0.5f;
            bool show = _stoveCounter.IsFried() && args.progressNormalized >= burnShowAmount;

            if (show)
            {
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

    // Update is called once per frame
    void Update()
    {

    }
}
