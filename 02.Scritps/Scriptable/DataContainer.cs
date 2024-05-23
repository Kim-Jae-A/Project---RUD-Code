using UnityEngine;


[CreateAssetMenu(fileName = "DataContainer", menuName = "ScriptableObject/DataContainer", order = 0)]
public class DataContainer : ScriptableObject
{
    [Header("유닛 데이터 컨테이너")]
    public UnitData[] CommonData;
    public UnitData[] UnCommonData;
    public UnitData[] RareData;
    public UnitData[] UniqueData;
    public UnitData[] EqicData;

    [Header("에너미 데이터 컨테이너")]
    public EnemyData[] RoundEnemy;
    public EnemyData[] MissionEnemy;
}



