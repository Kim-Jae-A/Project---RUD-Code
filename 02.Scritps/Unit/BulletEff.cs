using System.Collections;
using UnityEngine;

public class BulletEff : PoolAble
{
    int _range = 10;
    const float ATTACKDELEY = 0.2f;
    LayerMask _enemyMask;
    float _dmg;

    WaitForSeconds _delay = new WaitForSeconds(ATTACKDELEY);

    private void Awake()
    {
        _enemyMask = 1 << LayerMask.NameToLayer("Enemy");
    }

    void OnDisable()
    {
        transform.position = Vector3.zero;
        _dmg = 0;
    }

    IEnumerator DamageEnemy()
    {
        Damage();
        yield return _delay;
        Damage();
        yield return _delay;
        Damage();
        yield return _delay;
        Damage();
        yield return _delay;
        Damage();
        yield return _delay;
        Damage(); 
        yield return _delay;
        Damage();
        yield return _delay;
        Damage();
        yield return _delay;
        Damage();
        yield return _delay;
        Damage();
        yield return _delay;
        Damage();
        yield return _delay;
        Damage();
        yield return _delay;
        Damage();
        yield return _delay;
        Damage();
        yield return _delay;
        Damage();
        yield return _delay;
        Damage();

        ReleaseObject();
    }

    public void Damage()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, _range, _enemyMask);
        foreach (Collider enemy in enemiesInRange)
        {
            enemy.gameObject.GetComponent<Enemy>().hp -= _dmg;
        }
    }

    private void OnParticleSystemStopped()
    {
        ReleaseObject();
    }

    public void SetDamage(float dmg)
    {
        _dmg = dmg;
        _range = 10;
        Damage();
    }
    public void SetAreaDamage(float dmg)
    {
        _dmg = dmg;
        _range = 5;
        StartCoroutine(DamageEnemy());
    }
}
