using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
