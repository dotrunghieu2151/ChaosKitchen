using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button _playBtn;
    [SerializeField] private Button _quitBtn;

    private void Awake()
    {
        _playBtn.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.GameScene);
        });

        _quitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        Time.timeScale = 1f;
    }
    // Start is called before the first frame update
    void Start()
    {
        _playBtn.Select();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
