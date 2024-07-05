using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/UnitData")]
public class UnitData : ScriptableObject
{
    public string unitName;
    public UnitCode unitCode;
    public UnitGrade unitGrade;
    public AttackType attackType;
    public int range;
    public float attackSpeed;
    public float damage;
    public float upgradeDamage;
    public GameObject modle;
    public GameObject bullet;
    public float bulletSpeed;
    public GameObject bulletEff;
    public Sprite sprite;
    public string description;
}
