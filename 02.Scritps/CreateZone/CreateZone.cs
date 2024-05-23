using System;
using UnityEngine;

public class CreateZone : MonoBehaviour, ICreateZone
{
    public int ZoneIndex
    {
        get => _zoneIndex;
        set
        {
            _zoneIndex = value;
            UnitManager.createZone[value] = this;
        }
    }

    public UnitData unitData
    {
        get => _unitData;
        set
        {
            _unitData = value;
            OraSetting(value != null);
            HasUnit?.Invoke(value != null);
        }
    }

    public GameObject unit
    {
        get => _unit;
        set
        {
            _unit = value;
        }
    }

    public Transform mytransform
    {
        get => _mytransform;
    }

    int _zoneIndex;
    UnitData _unitData;
    Collider _collider;
    GameObject _unit;
    Transform _mytransform;
    GameObject _ora;

    public event Action<bool> HasUnit;


    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _mytransform = transform;
        HasUnit += value =>
        {
            _collider.enabled = !value;
        };
    }

    private void OraSetting(bool value)
    {
        if (!value)
        {
            if (_ora != null)
            {
                _ora.GetComponent<PoolAble>().ReleaseObject();
                _ora = null;
            }
            return;
        }
        if(_ora != null)
        {
            _ora.GetComponent<PoolAble>().ReleaseObject();
        }       
        _ora = ObjectPoolingManager.instance.GetGo("Ora");
        _ora.transform.SetParent(transform, false);
        _ora.transform.localPosition = Vector3.zero + new Vector3(0, 0.5f, 0);
        _ora.transform.localScale = new Vector3(5, 5, 5);
        ParticleSystem.MainModule main = _ora.GetComponent<ParticleSystem>().main;

        switch (_unitData.unitGrade)
        {
            case UnitGrade.Common:
                main.startColor = Color.white;
                break;
            case UnitGrade.UnCommon:
                main.startColor = new Color(0.5294f, 0.8078f, 0.9804f);
                break;
            case UnitGrade.Rare:
                main.startColor = Color.blue;
                break;
            case UnitGrade.Unique:
                main.startColor = new Color(1.0f, 0.0f, 1.0f);
                break;
            case UnitGrade.Eqic:
                main.startColor = new Color(1.0f, 0.6471f, 0.0f);
                break;
        }
    }
}
