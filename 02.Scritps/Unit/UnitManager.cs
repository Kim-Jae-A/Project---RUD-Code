using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;


public class UnitManager : SingletonMonoBase<UnitManager>
{
    LayerMask _zoneMask;
    LayerMask _unitMask;
    DataContainer _dataContainer;


    public bool createButtonOn
    {
        get => _createButtonOn;
        set
        {
            _createButtonOn = value;
        }
    }

    public bool seletUnitOn
    {
        get => _seletUnitOn;
        set
        {
            _seletUnitOn = value;
            sletUnitEvent.Invoke(value);
        }
    }

    UnitData _unitdata;
    int _unitGrade;

    bool _seletUnitOn;
    bool panelCheck;
    bool _createButtonOn;

    public Dictionary<(UnitCode, UnitGrade), int> _unitCount = new Dictionary<(UnitCode, UnitGrade), int>();
    Dictionary<(UnitCode, UnitGrade), List<ICreateZone>> _unitType = new Dictionary<(UnitCode, UnitGrade), List<ICreateZone>>();
    public static Dictionary<int, ICreateZone> createZone = new Dictionary<int, ICreateZone>();
    public Dictionary<UnitCode, int> unitUpgrade = new Dictionary<UnitCode, int>();
    Dictionary<UnitGrade, int> _gradeSelet = new Dictionary<UnitGrade, int>();

    public event Action<int> clickEvent;
    public event Action<bool> sletUnitEvent;
    public event Action<int, int> gradeCountChange;
    public event Action unitChanges;

    protected override void Awake()
    {
        base.Awake();
        _zoneMask = 1 << LayerMask.NameToLayer("UnitZone");
        _unitMask = 1 << LayerMask.NameToLayer("Unit");
        _dataContainer = Resources.Load<DataContainer>("DataContainer");
        ObjectPoolingManager.instance.AddObjPool("Ora", 44);

        for (int i = 0; i < Enum.GetNames(typeof(UnitCode)).Length - 1; i++)
        {
            unitUpgrade[(UnitCode)i] = 0;
            for (int j = 0; j < Enum.GetNames(typeof(UnitGrade)).Length - 1; j++)
            {
                _unitCount[((UnitCode)i, (UnitGrade)j)] = 0;
                _unitType[((UnitCode)i, (UnitGrade)j)] = new List<ICreateZone>();
            }
        }

        for (int i = 0; i < Enum.GetNames(typeof(UnitGrade)).Length - 1; i++)
        {
            _gradeSelet[(UnitGrade)i] = 0;
        }
    }

    private void Start()
    {
        for (int i = 0; i < _dataContainer.CommonData.Length; i++)
        {
            ObjectPoolingManager.instance.AddObjPool($"Unit/{_dataContainer.CommonData[i].modle.name}", 20);
            ObjectPoolingManager.instance.AddObjPool($"Bullet/{_dataContainer.CommonData[i].bullet.name}", 30);
            ObjectPoolingManager.instance.AddObjPool($"Bullet/Eff/{_dataContainer.CommonData[i].bulletEff.name}", 30);
        }
        //UIManager.instance.Get<UISetting>().settingChanged += value => panelCheck = value;
    }

    private void Update()
    {
        /*if (panelCheck)
        {
            return;
        }*/
        //#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            /*#elif UNITY_ANDROID
                    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                        {
                            return;
                        }
                        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            #endif     */
            if (Physics.Raycast(ray, out RaycastHit hitunit, float.PositiveInfinity, _unitMask))
            {
                clickEvent?.Invoke(hitunit.transform.GetComponentInParent<ICreateZone>().ZoneIndex);
            }
            if (_createButtonOn)
            {
                if (GameManager.instance.gold >= 100)
                {
                    if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, _zoneMask))
                    {
                        GameManager.instance.gold -= 100;
                        int index = hit.transform.GetComponent<CreateZone>().ZoneIndex;
                        if (_seletUnitOn)
                        {
                            CreateUnit(index, (UnitGrade)_unitGrade, _unitdata);
                            seletUnitOn = false;
                            return;
                        }
                        CreateUnit(index);
                    }
                }
            }           
        }
    }

    void CreateUnit(int index, UnitGrade grade = UnitGrade.None)
    {
        int random = Random.Range(0, _dataContainer.CommonData.Length);

        UnitGrade unit = grade + 1;

        switch (unit)
        {
            case UnitGrade.Common:
                createZone[index].unitData = _dataContainer.CommonData[random];
                break;
            case UnitGrade.UnCommon:
                createZone[index].unitData = _dataContainer.UnCommonData[random];
                break;
            case UnitGrade.Rare:
                createZone[index].unitData = _dataContainer.RareData[random];
                break;
            case UnitGrade.Unique:
                createZone[index].unitData = _dataContainer.UniqueData[random];
                break;
            case UnitGrade.Eqic:
                createZone[index].unitData = _dataContainer.EqicData[random];
                break;
        }
        UnitChanges(createZone[index].unitData.unitCode, createZone[index].unitData.unitGrade, 1);
        _unitType[(createZone[index].unitData.unitCode, createZone[index].unitData.unitGrade)].Add(createZone[index]);

        GameObject a = ObjectPoolingManager.instance.GetGo($"Unit/{createZone[index].unitData.modle.name}");
        a.GetComponent<Unit>().unitData = createZone[index].unitData;
        createZone[index].unit = a;
        a.transform.SetParent(createZone[index].mytransform, false);
        a.transform.localPosition = Vector3.zero;
        a.transform.localScale = new Vector3(8, 8, 8);
    }

    void CreateUnit(int index, UnitGrade grade, UnitData data)
    {
        switch (grade)
        {
            case UnitGrade.Common:
                createZone[index].unitData = data;
                break;
            case UnitGrade.UnCommon:
                createZone[index].unitData = data;
                break;
            case UnitGrade.Rare:
                createZone[index].unitData = data;
                break;
            case UnitGrade.Unique:
                createZone[index].unitData = data;
                break;
            case UnitGrade.Eqic:
                createZone[index].unitData = data;
                break;
        }
        UnitChanges(data.unitCode, grade, 1);
        _unitType[(data.unitCode, grade)].Add(createZone[index]);
        GradeSeletCount(grade, -1);
        GameObject a = ObjectPoolingManager.instance.GetGo($"Unit/{createZone[index].unitData.modle.name}");
        a.GetComponent<Unit>().unitData = createZone[index].unitData;
        createZone[index].unit = a;
        a.transform.SetParent(createZone[index].mytransform, false);
        a.transform.localPosition = Vector3.zero;
        a.transform.localScale = new Vector3(8, 8, 8);
    }

    public void EvolutionUnit(ICreateZone zone)
    {
        UnitData unitData = zone.unitData;
        if (_unitCount[(unitData.unitCode, unitData.unitGrade)] < 2)
        {
            return;
        }
        if (unitData.unitGrade == UnitGrade.Eqic)
        {
            return;
        }

        int random = 0;
        while (true)
        {
            random = Random.Range(0, _unitType[(unitData.unitCode, unitData.unitGrade)].Count);
            if (_unitType[(unitData.unitCode, unitData.unitGrade)][random].ZoneIndex != zone.ZoneIndex)
            {
                break;
            }
        }
        _unitType[(unitData.unitCode, unitData.unitGrade)][random].unitData = null;
        _unitType[(unitData.unitCode, unitData.unitGrade)][random].unit.GetComponent<UnitPool>().ReleaseObject();
        zone.unit.GetComponent<UnitPool>().ReleaseObject();
        _unitType[(unitData.unitCode, unitData.unitGrade)].RemoveAt(random);
        UnitChanges(unitData.unitCode, unitData.unitGrade, -2);

        for (int i = 0; i < _unitType[(unitData.unitCode, unitData.unitGrade)].Count; i++)
        {
            if (_unitType[(unitData.unitCode, unitData.unitGrade)][i].ZoneIndex == zone.ZoneIndex)
            {
                _unitType[(unitData.unitCode, unitData.unitGrade)].RemoveAt(i);
                break;
            }
        }
        CreateUnit(zone.ZoneIndex, zone.unitData.unitGrade);
    }

    public void SellUnit(ICreateZone zone)
    {
        UnitData unitData = zone.unitData;
        if (unitData == null)
        {
            return;
        }
        else if (unitData.unitGrade == UnitGrade.Eqic)
        {
            return;
        }

        GameManager.instance.gold += ((int)unitData.unitGrade + 1) * 50;
        zone.unit.GetComponent<UnitPool>().ReleaseObject();
        for (int i = 0; i < _unitType[(unitData.unitCode, unitData.unitGrade)].Count; i++)
        {
            if (_unitType[(unitData.unitCode, unitData.unitGrade)][i].ZoneIndex == zone.ZoneIndex)
            {
                _unitType[(unitData.unitCode, unitData.unitGrade)].RemoveAt(i);
                break;
            }
        }
        UnitChanges(unitData.unitCode, unitData.unitGrade, -1);
        zone.unitData = null;
    }

    public void UnitChanges(UnitCode unitCode, UnitGrade unitGrade, int count)
    {
        _unitCount[(unitCode, unitGrade)] += count;
        unitChanges?.Invoke();
    }

    public void UnitChoice(UnitData unit, int grade)
    {
        _unitdata = unit;
        _unitGrade = grade;
        seletUnitOn = true;
    }

    public int GradeSeletCount(UnitGrade grade, int num = 0)
    {
        if (num != 0)
        {
            _gradeSelet[grade] += num;
        }
        gradeCountChange?.Invoke((int)grade, _gradeSelet[grade]);

        return _gradeSelet[grade];
    }
}

