using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter _stoveCounter;
    private AudioSource _audioSource;

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
    }
}
