using System.Collections.Generic;
using UnityEngine;

public static class WaveBuilder
{
    public static List<MonsterData> BuildWave(WaveData waveData, List<MonsterData> monsterPool)
    {
        int waveValue = waveData.waveValue;
        int valueRange = waveData.GetValueRange();

        int usedValue = 0;
        List<MonsterData> spawnQueue = new List<MonsterData>();

        while (usedValue < waveValue)
        {
            List<MonsterData> valid = monsterPool.FindAll(m => m.spawnValue <= valueRange && m.spawnValue + usedValue <= waveValue);

            if (valid.Count == 0) break;

            MonsterData selected = valid[Random.Range(0, valid.Count)];
            spawnQueue.Add(selected);
            usedValue += selected.spawnValue;
        }

        return spawnQueue;
    }
}
