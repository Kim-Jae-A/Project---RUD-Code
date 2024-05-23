using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameOver : UIScreenBase
{
    Button _restart;
    Button _main;

    protected override void Awake()
    {
        base.Awake();
        _restart = transform.Find("Panel/Button - ReStart").GetComponent<Button>();
        _main = transform.Find("Panel/Button - Main").GetComponent<Button>();

        _restart.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("GameScene");
        });

        _main.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("LobbyScene");
        });
    }
}
