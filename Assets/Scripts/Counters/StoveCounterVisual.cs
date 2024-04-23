using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private GameObject _stoveOnGameObject;
    [SerializeField] private GameObject _stoveParticlesGameObject;
    [SerializeField] private StoveCounter _stoveCounter;
    // Start is called before the first frame update
    void Start()
    {
        _stoveCounter.OnStateChanged += (s, e) =>
        {
            bool showVisuals = e.state == StoveCounter.State.Fried || e.state == StoveCounter.State.Frying;
            if (showVisuals)
            {
                _stoveOnGameObject.SetActive(true);
                _stoveParticlesGameObject.SetActive(true);
            }
            else
            {
                _stoveOnGameObject.SetActive(false);
                _stoveParticlesGameObject.SetActive(false);
            }
        };
    }

    // Update is called once per frame
    void Update()
    {

    }
}
