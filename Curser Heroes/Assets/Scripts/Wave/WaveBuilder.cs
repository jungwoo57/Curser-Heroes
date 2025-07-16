using UnityEngine;
using System.Collections.Generic;

public static class WaveBuilder
{
    public static List<MonsterData> BuildWaveEntry(WaveEntry waveData, List<MonsterData> globalPool)
    {
        int waveValue = waveData.WaveValue;
        int valueRange = 2 + (waveData.wave / 10);
        int monsterCount = 10;

        List<MonsterData> usePool = globalPool;
        List<MonsterData> spawnQueue = new List<MonsterData>();
        int remainingValue = waveValue;

        for (int i = 0; i < monsterCount; i++)
        {
            int remainingMonsters = monsterCount - i - 1;
            int maxAllowed = Mathf.Min(valueRange, remainingValue - remainingMonsters);
            if (maxAllowed < 1) maxAllowed = 1;

            List<MonsterData> valid = usePool.FindAll(m => m != null && m.valueCost <= maxAllowed);
            if (valid.Count == 0) break;

            MonsterData selected = valid[Random.Range(0, valid.Count)];
            spawnQueue.Add(selected);
            remainingValue -= selected.valueCost;
        }

        return spawnQueue;
    }
}