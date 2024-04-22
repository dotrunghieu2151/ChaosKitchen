using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    [SerializeField] private ContainerCounter _containerCounter;
    private Animator animator;
    private const string OPEN_CLOSE = "OpenClose";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _containerCounter.OnPlayerGrabbedObject += ContainerCounter_OnPlayerGrabbedObject;
    }

    private void ContainerCounter_OnPlayerGrabbedObject(object sender, System.EventArgs e)
    {
        animator.SetTrigger(OPEN_CLOSE);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
