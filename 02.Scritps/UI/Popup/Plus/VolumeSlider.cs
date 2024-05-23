using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Button muteButton
    {
        get => _muteButton;
    }

    public Image image
    {
        get => _image;
    }

    public Sprite onSprite
    {
        get => _onSprite;
    }
    public Sprite offSprite
    {
        get => _offSprite;
    }
    public bool mute
    {
        get => _mute;
        set
        {
            _mute = value;
            muteHandler?.Invoke(value);
        }
    }

    Button _muteButton;
    Image _image;
    Sprite _onSprite;
    Sprite _offSprite;
    bool _mute;

    public event Action<bool> muteHandler;

    private void Awake()
    {
        _muteButton = transform.Find("Button").GetComponent<Button>();
        _image = _muteButton.GetComponent<Image>();
        _onSprite = _image.sprite;
        _offSprite = Resources.Load<Sprite>("OFF");
    }

    private void Start()
    {
        muteHandler += value =>
        {
            if (value)
            {
                _image.sprite = _offSprite;
            }
            else
            {
                _image.sprite = _onSprite;
            }
        };
    }
}
