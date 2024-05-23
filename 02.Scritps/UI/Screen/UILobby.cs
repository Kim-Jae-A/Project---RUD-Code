using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UILobby : UIScreenBase
{
    Button _start;
    Button _description;

    protected override void Awake()
    {
        base.Awake();
        _start = transform.Find("BG/Panel/Button - Start").GetComponent<Button>();
        _description = transform.Find("BG/Panel/Button - Description").GetComponent<Button>();

        _start.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("GameScene");
        });
    }
}
