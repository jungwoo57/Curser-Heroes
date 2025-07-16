using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "ScriptableObjects/MonsterData", order = 1)]
public class MonsterData : ScriptableObject
{
    public GameObject monsterPrefab;
    public int maxHP;
    public int valueCost;
    public float attackCooldown;
    public int damage;
    public int index; // 몬스터의 인덱스 번호
}   
