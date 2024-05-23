using System.Collections;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitData unitData
    {
        set
        {
            _unitData = value;
            Refresh();
        }
    }
    public GameObject bulletZone;

    UnitData _unitData;
    Animator _animator;
    float _range;
    float _attackSpeed;
    LayerMask _enemyMask;
    Transform _targetEnemy;
    bool _isStartSetTaget;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemyMask = 1 << LayerMask.NameToLayer("Enemy");
    }

    void Refresh()
    {
        _range = _unitData.range;
        _attackSpeed = _unitData.attackSpeed;
        _animator.SetFloat("AttackSpeed", _attackSpeed);
        _animator.SetBool("Attack", false);
        _targetEnemy = null;
        _isStartSetTaget = false;
    }

    private void Update()
    {
        if (!_isStartSetTaget && _targetEnemy == null)
        {
            _isStartSetTaget = true;
            StartCoroutine(FindTarget());
        }
    }

    IEnumerator FindTarget()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, _range, _enemyMask);

        float shortestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (Collider enemy in enemiesInRange)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy.transform;
            }
        }

        _targetEnemy = nearestEnemy;

        if (_targetEnemy != null)
        {
            yield return StartCoroutine(AttackTaget());
        }
        else
        {
            _isStartSetTaget = false;
            yield break;
        }
    }

    public IEnumerator AttackTaget()
    {
        if (_targetEnemy == null || ReferenceEquals(_targetEnemy, null))
        {
            _isStartSetTaget = false;
            _targetEnemy = null;
            yield break;
        }
        while (_targetEnemy != null)
        {
            if (_targetEnemy == null || ReferenceEquals(_targetEnemy, null))
            {
                _isStartSetTaget = false;
                _animator.SetBool("Attack", false);
                yield break;
            }
            if (_range < Vector3.Distance(transform.position, _targetEnemy.position))
            {
                _animator.SetBool("Attack", false);
                _isStartSetTaget = false;
                _targetEnemy = null;
                yield break;
            }
            else if (!_targetEnemy.gameObject.activeSelf)
            {
                _animator.SetBool("Attack", false);
                _isStartSetTaget = false;
                _targetEnemy = null;
                yield break;
            }
            _animator.SetBool("Attack", true);
            yield return null;
        }
    }


    public void Attack()
    {
        if (_targetEnemy == null || ReferenceEquals(_targetEnemy, null))
        {
            _isStartSetTaget = false;
            _animator.SetBool("Attack", false);
            return;
        }
        GameObject a = ObjectPoolingManager.instance.GetGo($"Bullet/{_unitData.bullet.name}");
        a.transform.position = bulletZone.transform.position;
        a.GetComponent<Bullet>().data = _unitData;
        a.GetComponent<Bullet>().tagetTransform = _targetEnemy;
        SoundManager.instance.SEFPlay(_unitData.modle.name);
    }

    public void LookAtTaget()
    {
        if (_targetEnemy == null)
        {
            return;
        }
        transform.LookAt(_targetEnemy.position - new Vector3(0, _targetEnemy.position.y, 0));
    }

    /*void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _range);
    }*/
}
