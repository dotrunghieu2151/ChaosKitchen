using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetDataManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BaseCounter.ResetStaticData();
        CuttingCounter.ResetStaticData();
        TrashCounter.ResetStaticData();
        PlayerMovement.ResetStaticData();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
