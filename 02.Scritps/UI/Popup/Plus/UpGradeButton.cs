using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpGradeButton : MonoBehaviour
{
    public Image modelSprite
    {
        get => _modelSprite;
    }

    public TMP_Text modelName
    {
        get => _modelName;

    }
    public TMP_Text price
    {
        get => _price;
    }

    public Button button
    {
        get => _button;
    }

    public int Index
    {
        get => _index;
        set => _index = value;
    }

    int _index;
    Image _modelSprite;
    TMP_Text _modelName;
    TMP_Text _price;
    Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _modelSprite = transform.Find("Image").GetComponent<Image>();
        _modelName = transform.Find("Text (TMP)").GetComponent<TMP_Text>();
        _price = transform.Find("Text (TMP) - Gem").GetComponent<TMP_Text>();
    }
}
