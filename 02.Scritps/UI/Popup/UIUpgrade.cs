using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgrade : UIPopupBase
{
    Button _closeButton;
    UpGradeButton[] _upgradeButtons;
    DataContainer _dataContainer;

    protected override void Awake()
    {
        base.Awake();
        _closeButton = transform.Find("BG/Panel/Text (TMP) - Title/Button - Close").GetComponent<Button>();
        _upgradeButtons = transform.Find("BG/Panel").GetComponentsInChildren<UpGradeButton>();
        _dataContainer = Resources.Load<DataContainer>("DataContainer");
        _closeButton.onClick.AddListener(Hide);
    }

    private void Start()
    {
        for(int i = 0; i < _upgradeButtons.Length; i++)
        {
            _upgradeButtons[i].Index = i;
            _upgradeButtons[i].modelName.text = _dataContainer.CommonData[i].unitName;
            _upgradeButtons[i].modelSprite.sprite = _dataContainer.CommonData[i].sprite;

        }

        foreach(var button  in _upgradeButtons)
        {
            button.price.text = $"{UnitManager.instance.unitUpgrade[_dataContainer.CommonData[button.Index].unitCode] * 2 + 20}";
            button.button.onClick.AddListener(() =>
            {
                int cost = UnitManager.instance.unitUpgrade[_dataContainer.CommonData[button.Index].unitCode] * 2 + 20;
                if (GameManager.instance.gem >= cost)
                {
                    GameManager.instance.gem -= cost;
                    UnitManager.instance.unitUpgrade[_dataContainer.CommonData[button.Index].unitCode]++;
                    button.price.text = $"{UnitManager.instance.unitUpgrade[_dataContainer.CommonData[button.Index].unitCode] * 2 + 20}";
                }
            });
        }
    }
}
