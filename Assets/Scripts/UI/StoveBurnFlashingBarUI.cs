using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnFlashingBarUI : MonoBehaviour
{
    [SerializeField] private StoveCounter _stoveCounter;
    private Animator _animator;
    private const string IS_FLASHING = "IsFlashing";
    // Start is called before the first frame update

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    void Start()
    {
        _stoveCounter.OnProgressChanged += (sender, args) =>
        {
            float burnShowAmount = 0.5f;
            bool show = _stoveCounter.IsFried() && args.progressNormalized >= burnShowAmount;

            _animator.SetBool(IS_FLASHING, show);
        };

        _animator.SetBool(IS_FLASHING, false);
    }
}
