using UnityEngine;


[CreateAssetMenu(fileName = "DataContainer", menuName = "ScriptableObject/DataContainer", order = 0)]
public class DataContainer : ScriptableObject
{
    [Header("���� ������ �����̳�")]
    public UnitData[] CommonData;
    public UnitData[] UnCommonData;
    public UnitData[] RareData;
    public UnitData[] UniqueData;
    public UnitData[] EqicData;

    [Header("���ʹ� ������ �����̳�")]
    public EnemyData[] RoundEnemy;
    public EnemyData[] MissionEnemy;
}



