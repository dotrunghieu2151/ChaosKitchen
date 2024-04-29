using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private Color _successColor;
    [SerializeField] private Color _failColor;
    [SerializeField] private Sprite _successSprite;
    [SerializeField] private Sprite _failSprite;

    private const string POPUP = "Popup";

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }


    // Start is called before the first frame update
    void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += (s, e) =>
        {
            gameObject.SetActive(true);
            _backgroundImage.color = _successColor;
            _iconImage.sprite = _successSprite;
            _messageText.text = "DELIVERY\nSUCCESS";
            _animator.SetTrigger(POPUP);
        };

        DeliveryManager.Instance.OnRecipeFailed += (s, e) =>
        {
            gameObject.SetActive(true);
            _backgroundImage.color = _failColor;
            _iconImage.sprite = _failSprite;
            _messageText.text = "DELIVERY\nFAILED";
            _animator.SetTrigger(POPUP);
        };
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
