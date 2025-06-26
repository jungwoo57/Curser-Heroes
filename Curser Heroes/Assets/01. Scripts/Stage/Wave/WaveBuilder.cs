using System.Collections.Generic;
using UnityEngine;

public static class WaveBuilder
{
    public static List<MonsterData> BuildWaveEntry(WaveEntry waveData, List<MonsterData> globalPool)
    {
        int waveValue = waveData.WaveValue;
        int valueRange = 2 + (waveData.wave / 10);
        int monsterCount = 10;

        // 🔄 사용할 몬스터 풀 선택 (override > global)
        List<MonsterData> usePool = waveData.HasOverrideEnemies ? waveData.overrideEnemies : globalPool;

        if (usePool == null || usePool.Count == 0)
        {
            Debug.LogError("Monster pool이 비어 있음. globalPool 또는 overrideEnemies 확인 필요");
            return new List<MonsterData>();
        }

        List<MonsterData> spawnQueue = new List<MonsterData>();
        int remainingValue = waveValue;

        for (int i = 0; i < monsterCount; i++)
        {
            int remainingMonsters = monsterCount - i - 1;
            int maxAllowed = Mathf.Min(valueRange, remainingValue - remainingMonsters);
            if (maxAllowed < 1) maxAllowed = 1;

            List<MonsterData> valid = usePool.FindAll(m => m != null && m.valueCost >= 1 && m.valueCost <= maxAllowed);
            if (valid.Count == 0)
            {
                MonsterData fallback = usePool.Find(m => m != null && m.valueCost == 1);
                if (fallback != null)
                {
                    spawnQueue.Add(fallback);
                    remainingValue -= fallback.valueCost;
                    continue;
                }

                Debug.LogError("valueCost = 1인 몬스터가 없어 10마리 조합 불가");
                break;
            }

            MonsterData selected = valid[Random.Range(0, valid.Count)];
            spawnQueue.Add(selected);
            remainingValue -= selected.valueCost;
        }

        return spawnQueue;
    }
}