using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter _stoveCounter;
    private AudioSource _audioSource;

    private float _warningSoundMaxTimer = 0.2f;

    private float _warningSoundTimer;
    private bool _playWarningSound = false;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _stoveCounter.OnStateChanged += (s, e) =>
        {
            bool playSound = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
            if (playSound)
            {
                _audioSource.Play();
            }
            else
            {
                _audioSource.Pause();
            }


        };
        _stoveCounter.OnProgressChanged += (s, e) =>
        {
            float burnShowAmount = 0.5f;
            _playWarningSound = _stoveCounter.IsFried() && e.progressNormalized >= burnShowAmount;
        };
    }

    private void Update()
    {
        if (_playWarningSound)
        {
            _warningSoundTimer -= Time.deltaTime;
            if (_warningSoundTimer <= 0f)
            {
                _warningSoundTimer = _warningSoundMaxTimer;

                SoundManager.Instance.PlayWarningSound(_stoveCounter.transform.position);
            }
        }
    }
}
