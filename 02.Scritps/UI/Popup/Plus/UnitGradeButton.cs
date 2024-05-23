using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitGradeButton : MonoBehaviour
{
    public int Index
    {
        get => _index;
        set => _index = value;
    }
    public int Count
    {
        get => _count;

        set
        {
            _count = value;
        }
    }

    public Button button
    {
        get => _button;
    }

    int _count;
    int _index;
    Button _button;
    TMP_Text _countText;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _countText = transform.Find("Text (TMP) - Count").GetComponent<TMP_Text>();
        _countText.text = "0";
        UnitManager.instance.gradeCountChange += (grade, value) =>
        {
            if (grade == _index)
            {
                _countText.text = $"{value}";
            }
        };
    }
}
