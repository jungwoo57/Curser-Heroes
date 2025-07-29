using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WaveGroupData")]
public class WaveGroupData : ScriptableObject
{
    [Header("전체 몬스터 풀")]
    public List<MonsterData> globalMonsterPool;

    [Header("개별 웨이브 설정")]
    public List<WaveEntry> waveEntries = new List<WaveEntry>();
}

[System.Serializable]
public class WaveEntry
{
    public int wave;

    [Tooltip("이 웨이브에 등장할 보스 (보스 웨이브일 때만 사용)")]
    public List<BossData> overrideBosses;

    public bool isBossWave;

    //public bool forceExactOverride; (추후에 사용될 수 있어 보류)

    public bool HasBosses => overrideBosses != null && overrideBosses.Count > 0;

    public int WaveValue => 30 + (wave - 1) * 2;

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