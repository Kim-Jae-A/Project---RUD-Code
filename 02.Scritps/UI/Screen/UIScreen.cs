using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIScreen : UIScreenBase
{
    TMP_Text _gold;
    TMP_Text _gem;
    TMP_Text _life;
    TMP_Text _round;
    TMP_Text _timer;
    TMP_Text _kill;
    Button _createButton;
    Button _convertButton;
    Button _missionButton;
    Button _upgradeButton;
    Button _seletButton;
    Button _damageButton;
    Button _settingButton;


    protected override void Awake()
    {
        base.Awake();

        _gold = transform.Find("Panel - Goods/Text (TMP) - Gold").GetComponent<TMP_Text>();
        _gem = transform.Find("Panel - Goods/Text (TMP) - Gem").GetComponent<TMP_Text>();
        _timer = transform.Find("Panel - Goods/Text (TMP) - Timer").GetComponent<TMP_Text>();
        _life = transform.Find("Panel - Life/Text (TMP) - Life").GetComponent<TMP_Text>();
        _kill = transform.Find("Panel - Life/Text (TMP) - Kill").GetComponent<TMP_Text>();
        _round = transform.Find("Panel - Life/Text (TMP) - Round").GetComponent<TMP_Text>();

        _settingButton = transform.Find("Button - Setting").GetComponent<Button>();

        _createButton = transform.Find("Panel - Info/Button - Create").GetComponent<Button>();
        _convertButton = transform.Find("Panel - Info/Button - Convert").GetComponent<Button>();
        _missionButton = transform.Find("Panel - Info/Button - Mission").GetComponent<Button>();
        _upgradeButton = transform.Find("Panel - Upgrade/Button - Upgrade").GetComponent<Button>();
        _seletButton = transform.Find("Panel - Upgrade/Button - Choice").GetComponent<Button>();
        _damageButton = transform.Find("Panel - Upgrade/Button - Damage").GetComponent<Button>();


        _settingButton.onClick.AddListener(() =>
        {
            UIManager.instance.Get<UISettingInGame>().Show();
        });

        _damageButton.onClick.AddListener(() =>
        {
            GameManager.instance.life--;
        });

        _seletButton.onClick.AddListener(() =>
        {
            UIManager.instance.Get<UIUnitChoices>().Show();
        });

        _createButton.onClick.AddListener(() =>
        {
            UnitManager.instance.createButtonOn = !UnitManager.instance.createButtonOn;

            if (UnitManager.instance.createButtonOn)
            {
                _createButton.GetComponent<Image>().color = Color.gray;
            }
            else
            {
                _createButton.GetComponent<Image>().color = Color.white;
            }
        });

        _convertButton.onClick.AddListener(() =>
        {
            if (GameManager.instance.gold >= 100)
            {
                GameManager.instance.gold -= 100;
                int random = Random.Range(1, 6);
                GameManager.instance.gem += random * 20;
            }
        });

        _missionButton.onClick.AddListener(() =>
        {
            UIManager.instance.Get<UIMission>().Show();
        });

        _upgradeButton.onClick.AddListener(() =>
        {
            UIManager.instance.Get<UIUpgrade>().Show();
        });

        GameManager.instance.goldChange += value =>
        {
            _gold.text = $"{value}";
        };

        GameManager.instance.gemChange += value =>
        {
            _gem.text = $"{value}";
        };

        GameManager.instance.lifeChange += value =>
        {
            if(value <= 0)
            {
                _life.text = "0";
                return;
            }
            _life.text = $"{value}";
        };

        GameManager.instance.roundChange += value =>
        {
            if(value == 50)
            {
                _round.text = $"클리어";
                GameManager.instance.gameClear = true;
                return;
            }
            _round.text = $"{value + 1}라운드";
        };

        GameManager.instance.killChange += value =>
        {
            _kill.text = $"{value}";
        };

        GameManager.instance.istimerChange += value =>
        {
            if (value)
            {
                StartCoroutine(TimerOn());
            }
            else
            {
                _timer.text = string.Empty;
            }
        };

        UnitManager.instance.sletUnitEvent += value =>
        {
            if (value)
            {
                _seletButton.transform.GetComponent<Image>().color = Color.gray;
            }
            else
            {
                _seletButton.transform.GetComponent<Image>().color = Color.white;
            }
        };
    }

    IEnumerator TimerOn()
    {
        float timer = GameManager.TURNTIMER;
        _timer.text = $"{GameManager.TURNTIMER}";
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            _timer.text = $"{timer:0}";
            yield return null;
        }
        _timer.text = string.Empty;
    }
}
