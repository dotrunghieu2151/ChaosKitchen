using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private Image _timerImage;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        float timerFill = GameManager.Instance.GetGamePlayingTimerNormalize();
        _timerImage.fillAmount = timerFill;
    }
}
