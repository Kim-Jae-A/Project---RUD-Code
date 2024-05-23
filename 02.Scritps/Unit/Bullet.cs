using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PoolAble
{
    public Transform tagetTransform
    {
        set => _tagetTransform = value;
    }

    public UnitData data
    {
        set
        {
            _data = value;
        }
    }

    UnitData _data;

    float releaseDistance = 2f;
    Transform _tagetTransform;
    Vector3 _vector;

    void Update()
    {
        if(_tagetTransform == null)
        {
            ReleaseObject();
            return;
        }
        else if(!_tagetTransform.gameObject.activeSelf)
        {
            ReleaseObject();
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, _tagetTransform.position);
        if (distanceToTarget <= releaseDistance)
        {
            HitEnemy();
            ReleaseObject();
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, _tagetTransform.position, _data.bulletSpeed * Time.deltaTime);
        _vector = _tagetTransform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(_vector).normalized;
    }   

    float SetDamage()
    {
        return _data.damage + _data.upgradeDamage * UnitManager.instance.unitUpgrade[_data.unitCode];
    }

    void HitEnemy()
    {
        if (_data.attackType != AttackType.Splash)
        {
            _tagetTransform.GetComponent<Enemy>().hp -= SetDamage();
        }
        GameObject a = ObjectPoolingManager.instance.GetGo($"Bullet/Eff/{_data.bulletEff.name}");
        a.transform.position = transform.position;
        if(_data.attackType == AttackType.Splash)
        {
            a.GetComponent<BulletEff>().SetDamage(SetDamage());
        }
        else if(_data.attackType == AttackType.Area)
        {
            a.GetComponent<BulletEff>().SetAreaDamage(SetDamage() / 5);
        }
    }
}
