using UnityEngine;
using UnityEngine.UI;

public class UIUnitChoices : UIPopupBase
{
    Button _closeButton;
    UnitChoiceButton[] _unitChoiceButtons;
    UnitGradeButton[] _unitGradeButtons;
    DataContainer _dataContainer;

    int _gradeIndex;
    UnitData _data;

    protected override void Awake()
    {
        base.Awake();
        _closeButton = transform.Find("BG/Panel/Text (TMP) - Title/Button - Close").GetComponent<Button>();
        _unitChoiceButtons = transform.Find("BG/Panel/Panel - Unit").GetComponentsInChildren<UnitChoiceButton>();
        _unitGradeButtons = transform.Find("BG/Panel/Panel - Grade").GetComponentsInChildren<UnitGradeButton>();
        _dataContainer = Resources.Load<DataContainer>("DataContainer");
        _closeButton.onClick.AddListener(() =>
        {
            UnitManager.instance.seletUnitOn = false;
            Hide();
        });
    }

    private void Start()
    {
        for (int i = 0; i < _unitChoiceButtons.Length; i++)
        {
            _unitChoiceButtons[i].Index = i;
            _unitChoiceButtons[i].modelName.text = _dataContainer.CommonData[i].unitName;
            _unitChoiceButtons[i].modelSprite.sprite = _dataContainer.CommonData[i].sprite;
        }

        for (int i = 0; i < _unitGradeButtons.Length; i++)
        {
            _unitGradeButtons[i].Index = i;
        }

        foreach (var button in _unitGradeButtons)
        {
            button.button.onClick.AddListener(() =>
            {
                if (UnitManager.instance.GradeSeletCount((UnitGrade)button.Index) >= 1)
                {
                    _gradeIndex = button.Index;
                    for (int i = 0; i < _unitChoiceButtons.Length; i++)
                    {
                        _unitChoiceButtons[i].button.interactable = true;
                    }
                }
            });
        }

        foreach (var button in _unitChoiceButtons)
        {
            button.button.onClick.AddListener(() =>
            {
                switch ((UnitGrade)_gradeIndex)
                {
                    case UnitGrade.Common:
                        _data = _dataContainer.CommonData[button.Index];
                        break;
                    case UnitGrade.UnCommon:
                        _data = _dataContainer.UnCommonData[button.Index];
                        break;
                    case UnitGrade.Rare:
                        _data = _dataContainer.RareData[button.Index];
                        break;
                    case UnitGrade.Unique:
                        _data = _dataContainer.UniqueData[button.Index];
                        break;
                    case UnitGrade.Eqic:
                        _data = _dataContainer.EqicData[button.Index];
                        break;
                }

                for (int i = 0; i < _unitChoiceButtons.Length; i++)
                {
                    if (_unitChoiceButtons[i].Index == button.Index)
                    {
                        _unitChoiceButtons[i].Image.color = Color.black;
                    }
                    else
                    {
                        _unitChoiceButtons[i].Image.color = Color.white;
                    }
                }
                UnitManager.instance.UnitChoice(_data, _gradeIndex);
                Hide();
            });
        }
    }
}
