using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObj;
    [SerializeField] private Image _barImage;
    private IHasProgress _hasProgress;
    // Start is called before the first frame update

    private float _targetProgress;
    private float _startProgress;
    private float _timer;

    private void Start()
    {
        _hasProgress = hasProgressGameObj.GetComponent<IHasProgress>();
        if (_hasProgress == null)
        {
            Debug.LogError("GameObject " + hasProgressGameObj + " should implement IHasProgress");
        }
        _hasProgress.OnProgressChanged += (sender, args) =>
        {
            _startProgress = _barImage.fillAmount;
            _targetProgress = args.progressNormalized;
            _timer = 0;
            if (args.progressNormalized == 0f)
            {
                Hide();
                _barImage.fillAmount = 0f;
            }
            else
            {
                Show();
            }
        };
        _barImage.fillAmount = 0f;
        _startProgress = 0f;
        _timer = 0;
        Hide();
    }

    private void Update()
    {
        if (_barImage.fillAmount < _targetProgress)
        {
            _timer += Time.deltaTime * 4f;
            _barImage.fillAmount = Mathf.Lerp(_startProgress, _targetProgress, _timer);
            if (_barImage.fillAmount == 1f)
            {
                Hide();
            }
        }
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
