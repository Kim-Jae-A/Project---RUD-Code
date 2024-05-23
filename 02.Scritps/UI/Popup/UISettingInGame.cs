using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISettingInGame : UIPopupBase
{
    const float ZERO = 0.001f;
    public bool settingPanel
    {
        get => _settingPanel;
        set
        {
            _settingPanel = value;
        }
    }

    bool _settingPanel;
    Button _closeButton;
    Button _speedButtons1;
    Button _speedButtons2;
    Button _speedButtons3;
    Button _restartButton;
    Button _lobbyButton;
    Button _exitButton;

    Slider _masterSlider;
    Slider _bgmSlider;
    Slider _sefSlider;

    //public event Action<bool> settingChanged;

    protected override void Awake()
    {
        base.Awake();
        _closeButton = transform.Find("Panel/SettingPanel/Button - Close").GetComponent<Button>();
        _speedButtons1 = transform.Find("Panel/SettingPanel/SpeedPanel/Button - speed1").GetComponent<Button>();
        _speedButtons2 = transform.Find("Panel/SettingPanel/SpeedPanel/Button - speed2").GetComponent<Button>();
        _speedButtons3 = transform.Find("Panel/SettingPanel/SpeedPanel/Button - speed3").GetComponent<Button>();
        _restartButton = transform.Find("Panel/SettingPanel/Button - ReStart").GetComponent<Button>();
        _lobbyButton = transform.Find("Panel/SettingPanel/Button - Main").GetComponent<Button>();
        _exitButton = transform.Find("Panel/SettingPanel/Button - Exit").GetComponent<Button>();
        _masterSlider = transform.Find("Panel/SettingPanel/Slider - Master").GetComponent<Slider>();
        _bgmSlider = transform.Find("Panel/SettingPanel/Slider - BGM").GetComponent<Slider>();
        _sefSlider = transform.Find("Panel/SettingPanel/Slider - SEF").GetComponent<Slider>();

        _closeButton.onClick.AddListener(() =>
        {
            Hide();
        });

        _speedButtons1.onClick.AddListener(() =>
        {
            GameManager.instance.timeScale = 1.0f;
        }); 
        _speedButtons2.onClick.AddListener(() =>
        {
            GameManager.instance.timeScale = 2.0f;
        }); 
        _speedButtons3.onClick.AddListener(() =>
        {
            GameManager.instance.timeScale = 3.0f;
        });

        _masterSlider.onValueChanged.AddListener(value =>
        {
            SoundManager.instance.master = value;
            _masterSlider.GetComponent<VolumeSlider>().mute = value <= ZERO ? true : false;
        });
        _bgmSlider.onValueChanged.AddListener(value =>
        {
            SoundManager.instance.bgm = value;
            _bgmSlider.GetComponent<VolumeSlider>().mute = value <= ZERO ? true : false;
        });
        _sefSlider.onValueChanged.AddListener(value =>
        {
            SoundManager.instance.sef = value;
            _sefSlider.GetComponent<VolumeSlider>().mute = value <= ZERO ? true : false;
        });

        _lobbyButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("LobbyScene");
        });

        _restartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("GameScene");
        });

        _exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    private void Start()
    {
        _masterSlider.GetComponent<VolumeSlider>().muteButton.onClick.AddListener(() =>
        {
            _masterSlider.GetComponent<VolumeSlider>().mute = !_masterSlider.GetComponent<VolumeSlider>().mute;
            if (_masterSlider.GetComponent<VolumeSlider>().mute)
            {
                SoundManager.instance.master = ZERO;
            }
            else
            {
                _masterSlider.value = _masterSlider.value > ZERO ? _masterSlider.value : 0.01f;
                SoundManager.instance.master = _masterSlider.value;
            }

        });
        _bgmSlider.GetComponent<VolumeSlider>().muteButton.onClick.AddListener(() =>
        {
            _bgmSlider.GetComponent<VolumeSlider>().mute = !_bgmSlider.GetComponent<VolumeSlider>().mute;
            if (_bgmSlider.GetComponent<VolumeSlider>().mute)
            {
                SoundManager.instance.bgm = ZERO;
            }
            else
            {
                _bgmSlider.value = _bgmSlider.value > ZERO ? _bgmSlider.value : 0.01f;
                SoundManager.instance.bgm = _bgmSlider.value;
            }

        });
        _sefSlider.GetComponent<VolumeSlider>().muteButton.onClick.AddListener(() =>
        {
            _sefSlider.GetComponent<VolumeSlider>().mute = !_sefSlider.GetComponent<VolumeSlider>().mute;
            if (_sefSlider.GetComponent<VolumeSlider>().mute)
            {
                SoundManager.instance.sef = ZERO;
            }
            else
            {
                _sefSlider.value = _sefSlider.value > ZERO ? _sefSlider.value : 0.01f;
                SoundManager.instance.sef = _sefSlider.value;
            }

        });
        _masterSlider.value = 1; 
        _bgmSlider.value = 1; 
        _sefSlider.value = 1;
    }

    public override void Show()
    {
        base.Show();
        settingPanel = true;
        Time.timeScale = 0;
    }

    public override void Hide()
    {
        base.Hide();
        settingPanel = false;
        Time.timeScale = GameManager.instance.timeScale;
    }
}
