using System.Collections.Generic;
using UnityEngine;

public static class WaveBuilder
{
    public static List<MonsterData> BuildWave(int wave, List<MonsterData> monsterPool)
    {
        int waveValue = WaveUtils.GetWaveValue(wave);
        int valueRange = 2 + (wave / 10);
        int monsterCount = 10;

        List<MonsterData> spawnQueue = new List<MonsterData>();
        int remainingValue = waveValue;

        for (int i = 0; i < monsterCount; i++)
        {
            int remainingMonsters = monsterCount - i - 1;
            int maxAllowed = Mathf.Min(valueRange, remainingValue - remainingMonsters);
            if (maxAllowed < 1) maxAllowed = 1;

            List<MonsterData> valid = monsterPool.FindAll(m => m != null && m.valueCost <= maxAllowed);
            if (valid.Count == 0) break;

            MonsterData selected = valid[Random.Range(0, valid.Count)];
            spawnQueue.Add(selected);
            remainingValue -= selected.valueCost;
        }

        return spawnQueue;
    }
}