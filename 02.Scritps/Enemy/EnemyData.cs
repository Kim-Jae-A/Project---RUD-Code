using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/EnemyData")]
public class EnemyData : ScriptableObject
{
    public EnemyType enemyType;
    public ArmourType armourType;
    public int index;
    public float hp;
    public float speed;
    public GameObject modle;
}
