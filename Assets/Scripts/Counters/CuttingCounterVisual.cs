using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    [SerializeField] private CuttingCounter _cuttingCounter;
    private Animator animator;
    private const string CUT = "Cut";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _cuttingCounter.OnCut += CuttingCounter_OnCut;
    }

    private void CuttingCounter_OnCut(object sender, System.EventArgs e)
    {
        animator.SetTrigger(CUT);
    }
}
