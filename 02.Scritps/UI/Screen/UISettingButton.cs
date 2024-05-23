using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettingButton : UIScreenBase
{
    Button button;

    protected override void Awake()
    {
        base.Awake();
        button = transform.Find("Button - Setting").GetComponent<Button>();

        button.onClick.AddListener(() =>
        {
            UIManager.instance.Get<UISetting>().Show();
        });
    }
}
