using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BossMonsterData", order = 3)]
public class BossData : ScriptableObject
{
    public GameObject BossPrefab;
    public int   maxHP;
    public float initialDelay = 2f;  // 보스 등장 후 첫 공격까지의 딜레이
    public float allpatternCooldown;  //패턴 전체 쿨타임
    public int[] patternCooldown;     // 각 패턴별 쿨타임
    public int WakeupStage;         // 보스가 깨어나는 단계
} 
