using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BossMonsterData", order = 3)]
public class BossData : ScriptableObject
{
    public int   maxHP;
    public float initialDelay = 2f;  // 보스 등장 후 첫 공격까지의 딜레이
    public float allpatternCooldown; //패턴 전체 쿨타임
    public int[] patternCooldown;     // 각 패턴별 쿨타임
    public int[] patternDamage;        // 각 패턴별 데미지
} 
