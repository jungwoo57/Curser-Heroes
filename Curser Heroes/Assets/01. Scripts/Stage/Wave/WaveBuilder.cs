using System.Collections.Generic;
using UnityEngine;

public static class WaveBuilder
{
    public static List<MonsterData> BuildWave(WaveData waveData, List<MonsterData> monsterPool)
    {
        int waveValue = waveData.waveValue;         // 예: 50
        int valueRange = waveData.GetValueRange();  // 예: 4
        int monsterCount = 10;

        List<MonsterData> spawnQueue = new List<MonsterData>();
        int remainingValue = waveValue;

        for (int i = 0; i < monsterCount; i++)
        {
            int remainingMonsters = monsterCount - i - 1;

            // 다음 몬스터들이 최소 1씩 가질 수 있도록 최대값 제한
            int maxAllowed = Mathf.Min(valueRange, remainingValue - remainingMonsters);
            if (maxAllowed < 1) maxAllowed = 1;

            // 조건에 맞는 몬스터 필터링
            List<MonsterData> valid = monsterPool.FindAll(m =>
                m.spawnValue >= 1 &&
                m.spawnValue <= maxAllowed
            );

            // 유효 몬스터가 없으면 fallback
            if (valid.Count == 0)
            {
                MonsterData fallback = monsterPool.Find(m => m.spawnValue == 1);
                if (fallback != null)
                {
                    spawnQueue.Add(fallback);
                    remainingValue -= fallback.spawnValue;
                    continue;
                }
                else
                {
                    Debug.LogError("spawnValue = 1 몬스터가 없어 10마리 조합 불가");
                    break;
                }
            }

            // 랜덤 선택
            MonsterData selected = valid[Random.Range(0, valid.Count)];
            spawnQueue.Add(selected);
            remainingValue -= selected.spawnValue;
        }

        return spawnQueue;
    }
}