using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private Image _barImage;
    [SerializeField] private CuttingCounter _cuttingCounter;
    // Start is called before the first frame update
    private void Start()
    {
        _cuttingCounter.OnProgressChanged += (sender, args) =>
        {
            _barImage.fillAmount = args.progressNormalized;
            if (args.progressNormalized == 0f || args.progressNormalized == 1f)
            {
                Hide();
            }
            else
            {
                Show();
            }
        };
        _barImage.fillAmount = 0f;
        Hide();
    }

    // Update is called once per frame
    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
