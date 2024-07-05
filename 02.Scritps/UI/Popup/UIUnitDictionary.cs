using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitDictionary : UIPopupBase
{
    Button _closeButton;
    Image _image;
    TMP_Text _description;

    protected override void Awake()
    {
        base.Awake();
        _closeButton = transform.Find("Panel/Button - Close").GetComponent<Button>();

        _closeButton.onClick.AddListener(() =>
        {
            Hide();
        });

        _image = transform.Find("Panel/Image - Unit/Unit").GetComponent<Image>();
        _description = transform.Find("Panel/Panel - Description/Text (TMP)").GetComponent<TMP_Text>();
    }

    public UIUnitDictionary Refresh(ICreateZone zone)
    {
        _image.sprite = zone.unitData.sprite;
        _description.text = zone.unitData.description;
        return this;
    }
}
