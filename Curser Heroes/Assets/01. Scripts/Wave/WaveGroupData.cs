using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WaveGroupData")]
public class WaveGroupData : ScriptableObject
{
    [Header("전체 웨이브 몬스터 풀")]
    public List<MonsterData> globalMonsterPool;

    public List<WaveEntry> waveEntries = new List<WaveEntry>();
}

[System.Serializable]
public class WaveEntry
{
    public int wave;
    [Tooltip("특정 웨이브에서만 사용되는 몬스터 (없으면 globalMonsterPool 사용)")]
    public List<MonsterData> overrideEnemies;

    public bool forceExactOverride; 

    public bool HasOverrideEnemies => overrideEnemies != null && overrideEnemies.Count > 0;

    public int WaveValue => 30 + (wave - 1) * 5;

    public int CalculateGoldReward()
    {
        return WaveValue * 10 + Random.Range(0, WaveValue + 1);
    }

    public int? TryGetJewelReward()
    {
        if (Random.value < 0.25f)
            return Random.Range(0, WaveValue + 1);
        return null;
    }
}
