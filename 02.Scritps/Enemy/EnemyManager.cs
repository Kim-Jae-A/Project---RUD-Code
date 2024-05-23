using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : SingletonMonoBase<EnemyManager>
{
    const int MAXGENCOUNT = 50;
    public Transform[] enemyTransforms;
    public Transform starting;

    DataContainer _dataContainer;
    WaitForSeconds _delay = new WaitForSeconds(1f);
    WaitUntil _waitRoundStart;
    int _creatCount;

    protected override void Awake()
    {
        base.Awake();
        _dataContainer = Resources.Load<DataContainer>("DataContainer");
        for (int i = 0; i < _dataContainer.RoundEnemy.Length; i++)
        {
            if (i == 9 || i == 19 || i == 29 || i == 39 || i == 49)
            {
                ObjectPoolingManager.instance.AddObjPool($"Round/{_dataContainer.RoundEnemy[i].modle.name}", 1);
                continue;
            }
            ObjectPoolingManager.instance.AddObjPool($"Round/{_dataContainer.RoundEnemy[i].modle.name}", 50);
        }
        for(int i = 0; i < _dataContainer.MissionEnemy.Length; i++)
        {
            ObjectPoolingManager.instance.AddObjPool($"Mission/{_dataContainer.MissionEnemy[i].modle.name}", 1);
        }
        _waitRoundStart = new WaitUntil(() => !GameManager.instance.roundStart);
    }

    

    private IEnumerator Start()
    {
        while (GameManager.instance.round < GameManager.MAXROUND - 1)
        {
            yield return _waitRoundStart;
            GameManager.instance.roundStart = true;
            yield return StartCoroutine(MobCreate());
        }

    }

    IEnumerator MobCreate()
    {
        while (_creatCount < MAXGENCOUNT)
        {
            GameObject a = ObjectPoolingManager.instance.GetGo($"Round/{_dataContainer.RoundEnemy[GameManager.instance.round].modle.name}");
            a.transform.position = starting.position;
            if (_dataContainer.RoundEnemy[GameManager.instance.round].enemyType == EnemyType.Boss)
            {
                a.GetComponent<Enemy>().enemyData = _dataContainer.RoundEnemy[GameManager.instance.round];
                _creatCount += MAXGENCOUNT;
                break;
            }
            a.GetComponent<Enemy>().enemyData = _dataContainer.RoundEnemy[GameManager.instance.round];
            _creatCount++;
            yield return _delay;
        }
        GameManager.instance.isgenFinsh = true;
        _creatCount = 0;        
    }
}
