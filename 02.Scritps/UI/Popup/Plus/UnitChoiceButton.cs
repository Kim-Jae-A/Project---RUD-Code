using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitChoiceButton : MonoBehaviour
{
    public TMP_Text modelName
    {
        get => _modelName;
    }

    public Button button
    {
        get => _button;
    }

    public Image modelSprite
    {
        get => _modelSprite;
    }

    public int Index
    {
        get => _index;
        set => _index = value;
    }
    
    public Image Image
    {
        get => _image;
    }

    Image _image;
    Image _modelSprite;
    int _index;
    TMP_Text _modelName;
    Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
        _modelSprite = transform.Find("Image").GetComponent<Image>();
        _modelName = transform.Find("Text (TMP)").GetComponent<TMP_Text>();
        button.interactable = false;
    }

    private void Start()
    {
        UnitManager.instance.sletUnitEvent += value =>
        {
            if (value == false)
            {
                _image.color = Color.white;
                button.interactable = false;
            }
        };
    }
}
