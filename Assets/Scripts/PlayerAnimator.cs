using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";

    private Animator _animator;
    [SerializeField] private PlayerMovement _playerMovement;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool(IS_WALKING, _playerMovement.IsWalking());
    }
}
