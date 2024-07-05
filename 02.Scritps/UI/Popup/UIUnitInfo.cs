using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitInfo : UIPopupBase
{
    TMP_Text _type;
    TMP_Text _grade;
    TMP_Text _attackSpeed;
    TMP_Text _damage;
    Image _unitImage;
    Button _evolutionButton;
    Button _sellButton;
    Button _closeButton;
    Button _infoButton;

    ICreateZone _createZone;

    protected override void Awake()
    {
        base.Awake();
        
        _type = transform.Find("Panel/Text (TMP) - UnitCode").GetComponent<TMP_Text>();
        _grade = transform.Find("Panel/Text (TMP) - UnitGrade").GetComponent<TMP_Text>();
        _attackSpeed = transform.Find("Panel/Text (TMP) - AttackSpeed").GetComponent<TMP_Text>();
        _damage = transform.Find("Panel/Text (TMP) - Damage").GetComponent<TMP_Text>();
        _evolutionButton = transform.Find("Panel/Button - Evolution").GetComponent<Button>();
        _sellButton = transform.Find("Panel/Button - Sell").GetComponent<Button>();
        _closeButton = transform.Find("Panel/Button - Close").GetComponent<Button>();
        _unitImage = transform.Find("Panel/Image - Unit/Unit").GetComponent<Image>();
        _infoButton = transform.Find("Panel/Button - Info").GetComponent<Button>();

        UnitManager.instance.clickEvent += value =>
        {
            Show();
            _createZone = UnitManager.createZone[value];
            _unitImage.sprite = _createZone.unitData.sprite;
            _type.text = $"{_createZone.unitData.unitName} (+{UnitManager.instance.unitUpgrade[_createZone.unitData.unitCode]})";
            _grade.text = $"{_createZone.unitData.unitGrade}";
            _attackSpeed.text = $"°ø¼Ó : {_createZone.unitData.attackSpeed}";
            _damage.text = $"{_createZone.unitData.damage} (+{_createZone.unitData.upgradeDamage * UnitManager.instance.unitUpgrade[_createZone.unitData.unitCode]})";
        };

        _evolutionButton.onClick.AddListener(() =>
        {          
            UnitManager.instance.EvolutionUnit(_createZone);
            _createZone = null;
            Hide();
        });

        _sellButton.onClick.AddListener(() =>
        {
            UnitManager.instance.SellUnit(_createZone);
            _createZone = null;
            Hide();
        });

        _infoButton.onClick.AddListener(() =>
        {
            UIManager.instance.Get<UIUnitDictionary>().Refresh(_createZone).Show();
        });

        _closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
    }
}
