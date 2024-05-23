using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : PoolAble
{
    public EnemyData enemyData
    {
        get => _enemyData;
        set
        {
            _enemyData = value;
            Refresh();
        }
    }

    public Transform[] tagetTransform
    {
        set => _tagetTransform = value;
    }

    public float hp
    {
        get => _hp;
        set
        {
            _hp = value;
            hpChange?.Invoke();
        }
    }

    Slider _hpslider;
    Transform[] _tagetTransform;
    EnemyData _enemyData;
    EnemyType _enemyType;
    ArmourType _armourType;
    int _index;
    int _nowPos;
    float _hp;
    float _speed;
    Vector3 _vector;
    float releaseDistance = 0.5f;
    Transform _cameraPos;

    public event Action hpChange;


    private void Awake()
    {
        _hpslider = transform.Find("Canvas - HpSlider/Slider").GetComponent<Slider>();
        hpChange += () => HpChanges();

    }

    void Refresh()
    {
        _cameraPos = Camera.main.transform;
        _tagetTransform = EnemyManager.instance.enemyTransforms;
        _enemyType = _enemyData.enemyType;
        _armourType = _enemyData.armourType;
        _index = _enemyData.index;
        hp = _enemyData.hp;
        _speed = _enemyData.speed;
        _nowPos = 0;
        _hpslider.maxValue = _hp;
        _hpslider.value = _hp;
        GameManager.instance.monsterCount++;
    }


    private void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, _tagetTransform[_nowPos].position);
        if (distanceToTarget <= releaseDistance)
        {
            _nowPos++;
            if(_nowPos == _tagetTransform.Length)
            {
                switch (_enemyType)
                {
                    case EnemyType.Boss:
                        if(_enemyData.index == 5)
                        {
                            GameManager.instance.life = 0;
                            break;
                        }
                        GameManager.instance.life -= 30;
                        break;
                    case EnemyType.Mission:
                        GameManager.instance.life -= 20;
                        break;
                    case EnemyType.Round:
                        GameManager.instance.life--;
                        break;
                }
                GameManager.instance.monsterCount--;
                ReleaseObject();
                return;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, _tagetTransform[_nowPos].position, _speed * Time.deltaTime);
        _vector = _tagetTransform[_nowPos].position - transform.position;
        transform.rotation = Quaternion.LookRotation(_vector).normalized;
        _hpslider.transform.LookAt(transform.position + _cameraPos.rotation * Vector3.forward, _cameraPos.rotation * Vector3.up);
    }

    void HpChanges()
    {
        if (_hp <= 0)
        {
            Reward();
            ReleaseObject();
        }

        _hpslider.value = _hp;
        
    }

    void Reward()
    {
        switch (_enemyType)
        {
            case EnemyType.Boss:
                UnitManager.instance.GradeSeletCount(UnitGrade.Rare, 1);
                UIManager.instance.Get<UIMessage>().SetMessage("보스 처치 (+ 레어 선택권 1)");
                break;
            case EnemyType.Mission:
                GameManager.instance.gold += _index * 100;
                break;
        }
        GameManager.instance.kill++;
        GameManager.instance.monsterCount--;
    }
}

