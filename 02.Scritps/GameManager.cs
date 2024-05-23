using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBase<GameManager>
{
    public const int TURNTIMER = 20;
    public const int MAXROUND = 50;
    public int gold
    {
        get => _gold;
        set
        {
            _gold = value;
            goldChange?.Invoke(value);
        }
    }
    public int gem
    {
        get => _gem;
        set
        {
            _gem = value;
            gemChange?.Invoke(value);
        }
    }

    public int life
    {
        get => _life;
        set
        {
            _life = value;
            lifeChange?.Invoke(value);
        }
    }

    public float timeScale
    {
        get => _timeScale;
        set
        {
            _timeScale = value;
        }
    }

    public int monsterCount
    {
        get => _monsterCount;
        set
        {
            _monsterCount = value;
        }
    }

    public int round
    {
        get => _round;
        private set
        {
            _round = value;
            roundChange?.Invoke(value);
        }
    }

    public bool roundStart
    {
        get => _roundStart;
        set
        {
            _roundStart = value;
        }
    }

    public bool isgenFinsh
    {
        set
        {
            _isgenFinsh = value;
            isgenFinshEvent.Invoke(value);
        }
    }

    public bool istimerOn
    {
        get => _istimerOn;
        set
        {
            _istimerOn = value;
            istimerChange?.Invoke(value);
        }
    }

    public int kill
    {
        get => _kill;
        set
        {
            _kill = value;
            killChange?.Invoke(value);
        }
    }

    bool _istimerOn;
    bool _isgenFinsh;
    bool _roundStart;
    int _monsterCount;
    int _gold;
    int _gem;
    int _life;
    int _round;
    int _kill;
    float _timeScale;

    WaitForSeconds _delay = new WaitForSeconds(TURNTIMER);
    WaitUntil _mobZeroWait;

    public event Action<bool> istimerChange;
    public event Action<int> roundChange;
    public event Action<int> goldChange;
    public event Action<int> gemChange;
    public event Action<int> lifeChange;
    public event Action<int> killChange;
    public event Action<bool> isgenFinshEvent;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        gem = 0;
        monsterCount = 0;
        gold = 400;
        life = 50;
        timeScale = 1;
        Time.timeScale = timeScale;
        round = 0;
        kill = 0;
        _mobZeroWait = new WaitUntil(() => _monsterCount == 0);

        isgenFinshEvent += value =>
        {
            if (value)
                StartCoroutine(MobCheck());
        };

        #region 미션
        #region 올 커먼
        Action allCommon = null;
        allCommon = () =>
        {
            bool[] checks = new bool[Enum.GetNames(typeof(UnitCode)).Length - 1];
            for (int i = 0; i < Enum.GetNames(typeof(UnitCode)).Length - 1; i++)
                checks[i] = UnitManager.instance._unitCount[((UnitCode)i, UnitGrade.Common)] > 0;

            if (checks.All(check => check))
            {
                gold += 300;
                UIManager.instance.Get<UIMessage>().SetMessage("All Commons (+ 골드 300)");
                UnitManager.instance.unitChanges -= allCommon;
            }
        };
        UnitManager.instance.unitChanges += allCommon;
        #endregion
        #region 총잡이들
        Action gunners = null;
        gunners = () =>
        {
            bool[] checks = new bool[2];
            checks[0] = UnitManager.instance._unitCount[(UnitCode.Mechanic, UnitGrade.UnCommon)] > 0;
            checks[1] = UnitManager.instance._unitCount[(UnitCode.Gunner, UnitGrade.UnCommon)] > 0;

            if (checks.All(check => check))
            {
                gold += 300;
                UIManager.instance.Get<UIMessage>().SetMessage("총잡이들 (+ 골드 400)");
                UnitManager.instance.unitChanges -= gunners;
            }
        };
        UnitManager.instance.unitChanges += gunners;
        #endregion
        #region 레어세븐
        Action rares = null;
        rares = () =>
        {
            int count = 0;
            for (int i = 0; i < Enum.GetNames(typeof(UnitCode)).Length - 1; i++)
                count += UnitManager.instance._unitCount[((UnitCode)i, UnitGrade.Rare)];

            if (count >= 7)
            {
                gold += 300;
                UIManager.instance.Get<UIMessage>().SetMessage("레어세븐 (+ 골드 300)");
                UnitManager.instance.unitChanges -= rares;
            }
        };
        UnitManager.instance.unitChanges += rares;
        #endregion
        #region 악운도 운이야
        Action unLuckisLuck = null;
        unLuckisLuck = () =>
        {
            int count = 0;
            for (int i = 0; i < Enum.GetNames(typeof(UnitCode)).Length - 1; i++)
                count += UnitManager.instance._unitCount[((UnitCode)i, UnitGrade.Unique)];

            if (count >= 6)
            {
                gold += 300;
                UIManager.instance.Get<UIMessage>().SetMessage("악운도 운이야 (+ 골드 300)");
                UnitManager.instance.unitChanges -= unLuckisLuck;
            }
        };
        UnitManager.instance.unitChanges += unLuckisLuck;
        #endregion
        #region 히든 이벤트
        #region 존버의 신
        Action<int, int> gradeCountChangeHandler = null;
        gradeCountChangeHandler = (grade, value) =>
        {
            if (grade == (int)UnitGrade.Rare)
            {
                if (value >= 5)
                {
                    UnitManager.instance.GradeSeletCount(UnitGrade.Unique, 1);
                    UIManager.instance.Get<UIMessage>().SetMessage("존버의 신 (+ 유니크 선택권 1)");
                    UnitManager.instance.gradeCountChange -= gradeCountChangeHandler;
                }
            }
        };
        UnitManager.instance.gradeCountChange += gradeCountChangeHandler;
        #endregion
        #region 프로토타입
        Action prototype = null;
        prototype = () =>
        {
            bool[] checks = new bool[8];
            checks[0] = UnitManager.instance._unitCount[(UnitCode.Devil, UnitGrade.Common)] > 0;
            checks[1] = UnitManager.instance._unitCount[(UnitCode.Mechanic, UnitGrade.Common)] > 0;
            checks[2] = UnitManager.instance._unitCount[(UnitCode.Devil, UnitGrade.UnCommon)] > 0;
            checks[3] = UnitManager.instance._unitCount[(UnitCode.Mechanic, UnitGrade.UnCommon)] > 0;
            checks[4] = UnitManager.instance._unitCount[(UnitCode.Devil, UnitGrade.Rare)] > 0;
            checks[5] = UnitManager.instance._unitCount[(UnitCode.Mechanic, UnitGrade.Rare)] > 0;
            checks[6] = UnitManager.instance._unitCount[(UnitCode.Devil, UnitGrade.Unique)] > 0;
            checks[7] = UnitManager.instance._unitCount[(UnitCode.Mechanic, UnitGrade.Unique)] > 0;

            if (checks.All(check => check))
            {
                UnitManager.instance.GradeSeletCount(UnitGrade.Unique, 1);
                UIManager.instance.Get<UIMessage>().SetMessage("프로토타입부터 있었다 (+ 유니크 선택권 1)");
                UnitManager.instance.unitChanges -= prototype;
            }
        };
        UnitManager.instance.unitChanges += prototype;
        #endregion
        #region 천사와 악마
        Action angelDevil = null;
        angelDevil = () =>
        {
            bool[] checks = new bool[8];
            checks[0] = UnitManager.instance._unitCount[(UnitCode.Devil, UnitGrade.Common)] > 0;
            checks[1] = UnitManager.instance._unitCount[(UnitCode.Angle, UnitGrade.Common)] > 0;
            checks[2] = UnitManager.instance._unitCount[(UnitCode.Devil, UnitGrade.UnCommon)] > 0;
            checks[3] = UnitManager.instance._unitCount[(UnitCode.Angle, UnitGrade.UnCommon)] > 0;
            checks[4] = UnitManager.instance._unitCount[(UnitCode.Devil, UnitGrade.Rare)] > 0;
            checks[5] = UnitManager.instance._unitCount[(UnitCode.Angle, UnitGrade.Rare)] > 0;
            checks[6] = UnitManager.instance._unitCount[(UnitCode.Devil, UnitGrade.Unique)] > 0;
            checks[7] = UnitManager.instance._unitCount[(UnitCode.Angle, UnitGrade.Unique)] > 0;

            if (checks.All(check => check))
            {
                UnitManager.instance.GradeSeletCount(UnitGrade.Unique, 1);
                UIManager.instance.Get<UIMessage>().SetMessage("천사와 악마 (+ 유니크 선택권 1)");
                UnitManager.instance.unitChanges -= angelDevil;
            }
        };
        UnitManager.instance.unitChanges += angelDevil;
        #endregion
        #region 수집광
        Action collection = null;
        collection = () =>
        {
            bool[] checks = new bool[Enum.GetNames(typeof(UnitCode)).Length - 2];
            for (int i = 0; i < Enum.GetNames(typeof(UnitCode)).Length - 2; i++)
                checks[i] = UnitManager.instance._unitCount[((UnitCode)i, UnitGrade.Unique)] > 0;

            if (checks.All(check => check))
            {
                UnitManager.instance.GradeSeletCount(UnitGrade.Rare, 3);
                UIManager.instance.Get<UIMessage>().SetMessage("수집광 (+ 레어 선택권 3)");
                UnitManager.instance.unitChanges -= collection;
            }
        };
        UnitManager.instance.unitChanges += collection;
        #endregion
        #region 피규어 수집
        Action essential = null;
        essential = () =>
        {
            bool[] checks = new bool[6];
            checks[0] = UnitManager.instance._unitCount[(UnitCode.Angle, UnitGrade.UnCommon)] > 1;
            checks[1] = UnitManager.instance._unitCount[(UnitCode.Magician, UnitGrade.UnCommon)] > 1;
            checks[2] = UnitManager.instance._unitCount[(UnitCode.Gunner, UnitGrade.Rare)] > 1;
            checks[3] = UnitManager.instance._unitCount[(UnitCode.BloodElf, UnitGrade.Rare)] > 1;
            checks[4] = UnitManager.instance._unitCount[(UnitCode.Elf, UnitGrade.Unique)] > 1;
            checks[5] = UnitManager.instance._unitCount[(UnitCode.Warrior, UnitGrade.Unique)] > 1;

            if (checks.All(check => check))
            {
                UnitManager.instance.GradeSeletCount(UnitGrade.Eqic, 1);
                UIManager.instance.Get<UIMessage>().SetMessage("피규어 수집 (+ 에픽 선택권 1)");
                UnitManager.instance.unitChanges -= essential;
            }
        };
        UnitManager.instance.unitChanges += essential;
        #endregion
        #region 마법사들
        Action elfs1 = null;
        elfs1 = () =>
        {
            bool[] checks = new bool[4];
            checks[0] = UnitManager.instance._unitCount[(UnitCode.Magician, UnitGrade.Rare)] > 2;
            checks[1] = UnitManager.instance._unitCount[(UnitCode.BloodElf, UnitGrade.Rare)] > 2;
            checks[2] = UnitManager.instance._unitCount[(UnitCode.Magician, UnitGrade.UnCommon)] > 2;
            checks[3] = UnitManager.instance._unitCount[(UnitCode.BloodElf, UnitGrade.UnCommon)] > 2;

            if (checks.All(check => check))
            {
                UnitManager.instance.GradeSeletCount(UnitGrade.Rare, 2);
                UIManager.instance.Get<UIMessage>().SetMessage("마법사입니다만 (+ 레어 선택권 2)");
                UnitManager.instance.unitChanges -= elfs1;
            }
        };
        UnitManager.instance.unitChanges += elfs1;
        #endregion
        #region 엘프랍니다
        Action elfs = null;
        elfs = () =>
        {
            bool[] checks = new bool[2];
            checks[0] = UnitManager.instance._unitCount[(UnitCode.Elf, UnitGrade.Eqic)] > 0;
            checks[1] = UnitManager.instance._unitCount[(UnitCode.BloodElf, UnitGrade.Unique)] > 1;

            if (checks.All(check => check))
            {
                gem += 550;
                UIManager.instance.Get<UIMessage>().SetMessage("엘프랍니다 (+ 젬 550)");
                UnitManager.instance.unitChanges -= elfs;
            }
        };
        UnitManager.instance.unitChanges += elfs;
        #endregion
        #region 전사의 선택
        Action warrior = null;
        warrior = () =>
        {
            bool checks = UnitManager.instance._unitCount[(UnitCode.Warrior, UnitGrade.Unique)] > 2;

            if (checks)
            {
                gold += 400;
                gem += 400;
                UIManager.instance.Get<UIMessage>().SetMessage("전사의 선택 (+ 골드 400 젬 400)");
                UnitManager.instance.unitChanges -= warrior;
            }
        };
        UnitManager.instance.unitChanges += warrior;
        #endregion
        #region 여자총잡이
        Action womangunner = null;
        womangunner = () =>
        {
            bool checks = UnitManager.instance._unitCount[(UnitCode.Gunner, UnitGrade.Unique)] > 2;

            if (checks)
            {
                gold += 400;
                gem += 400;
                UIManager.instance.Get<UIMessage>().SetMessage("여자 총잡이 (+ 골드 400 젬 400)");
                UnitManager.instance.unitChanges -= womangunner;
            }
        };
        UnitManager.instance.unitChanges += womangunner;
        #endregion
        #endregion
        #endregion
        #region 킬수이벤트
        Action<int> killChangeHandler = null;
        Action<int> killChangeHandler2 = null;
        Action<int> killChangeHandler3 = null;
        Action<int> killChangeHandler4 = null;
        Action<int> killChangeHandler5 = null;
        killChangeHandler = value =>
        {
            if (value >= 450)
            {
                gold += 200;
                UIManager.instance.Get<UIMessage>().SetMessage("좋은 출발 (+ 200골드)");
                killChange -= killChangeHandler;
                killChange += killChangeHandler2;
            }
        };
        killChange += killChangeHandler;
        killChangeHandler2 = value =>
        {
            if (value >= 850)
            {
                gold += 300;
                UIManager.instance.Get<UIMessage>().SetMessage("꽤 할지도..? (+ 300 골드)");
                killChange -= killChangeHandler2;
                killChange += killChangeHandler3;
            }
        };
        killChangeHandler3 = value =>
        {
            if (value >= 1300)
            {
                gold += 400;
                UIManager.instance.Get<UIMessage>().SetMessage("이게나야 (+ 400 골드)");
                killChange -= killChangeHandler3;
                killChange += killChangeHandler4;
            }
        };
        killChangeHandler4 = value =>
        {
            if (value >= 1650)
            {
                UnitManager.instance.GradeSeletCount(UnitGrade.Rare, 2);
                UIManager.instance.Get<UIMessage>().SetMessage("그냥 고수임 (+ 레어 선택권 2)");
                killChange -= killChangeHandler4;
                killChange += killChangeHandler5;
            }
        };
        killChangeHandler5 = value =>
        {
            if (value >= 2150)
            {
                gem += 300;
                UIManager.instance.Get<UIMessage>().SetMessage("클리어가 코앞 (+ 젬 300)");
                killChange -= killChangeHandler5;
            }
        };
        #endregion
        #region 라이프이벤트
        Action<int> lifetenunderCheck = null;
        lifetenunderCheck += value =>
        {
            if (value <= 10)
            {
                gem += 400;
                UIManager.instance.Get<UIMessage>().SetMessage("거의 죽기 직전 (+ 젬 400)");
                lifeChange -= lifetenunderCheck;
            }
        };
        lifeChange += lifetenunderCheck;
        lifeChange += value =>
        {
            if (value <= 0)
            {
                SceneManager.LoadScene("GameOver");
            }
        };
        #endregion
    }

    IEnumerator MobCheck()
    {
        yield return _mobZeroWait;
        StartCoroutine(RoundLogic());
    }

    IEnumerator RoundLogic()
    {
        istimerOn = true;
        gold += 200;
        yield return _delay;
        istimerOn = false;
        round++;
        _isgenFinsh = false;
        roundStart = false;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            gold += 1000;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            gem += 1000;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UnitManager.instance.GradeSeletCount(UnitGrade.Common, 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UnitManager.instance.GradeSeletCount(UnitGrade.UnCommon, 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UnitManager.instance.GradeSeletCount(UnitGrade.Rare, 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            UnitManager.instance.GradeSeletCount(UnitGrade.Unique, 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            UnitManager.instance.GradeSeletCount(UnitGrade.Eqic, 1);
        }
    }
}
