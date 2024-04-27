using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private PlayerMovement _player;
    private float _footStepTimer;
    private float _footStepTimeMax = 0.1f;
    // Start is called before the first frame update
    private void Awake()
    {
        _player = GetComponent<PlayerMovement>();
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _footStepTimer -= Time.deltaTime;
        if (_footStepTimer <= 0)
        {
            _footStepTimer = _footStepTimeMax;

            if (_player.IsWalking())
            {
                SoundManager.Instance.PlayFootstepSound(_player.transform.position, 1f);
            }
        }
    }
}
