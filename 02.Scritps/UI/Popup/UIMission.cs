using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIMission : UIPopupBase
{
    Button _mission1;
    Button _mission2;
    Button _mission3;
    Button _close;
    DataContainer _dataContainer;

    WaitForSeconds _sleep = new WaitForSeconds(360);

    protected override void Awake()
    {
        base.Awake();
        _dataContainer = Resources.Load<DataContainer>("DataContainer");
        _mission1 = transform.Find("BG/Panel/Button - Mob1").GetComponent<Button>();
        _mission2 = transform.Find("BG/Panel/Button - Mob2").GetComponent<Button>();
        _mission3 = transform.Find("BG/Panel/Button - Mob3").GetComponent<Button>();
        _close = transform.Find("BG/Panel/Text (TMP) - Title/Button - Close").GetComponent<Button>();

        _close.onClick.AddListener(() => Hide());

        _mission1.onClick.AddListener(() =>
        {
            MissionMobGen(0);
            StartCoroutine(ButtonOFF(_mission1));
        });
        _mission2.onClick.AddListener(() =>
        {
            MissionMobGen(1);
            StartCoroutine(ButtonOFF(_mission2));
        });
        _mission3.onClick.AddListener(() =>
        {
            MissionMobGen(2);
            StartCoroutine(ButtonOFF(_mission3));
        });
    }

    void MissionMobGen(int num)
    {
        GameObject a = ObjectPoolingManager.instance.GetGo($"Mission/{_dataContainer.MissionEnemy[num].modle.name}");
        a.transform.position = EnemyManager.instance.starting.position;
        a.GetComponent<Enemy>().enemyData = _dataContainer.MissionEnemy[num];
    }
    IEnumerator ButtonOFF(Button button)
    {
        button.interactable = false;
        yield return _sleep;
        button.interactable = true;
    }
}
