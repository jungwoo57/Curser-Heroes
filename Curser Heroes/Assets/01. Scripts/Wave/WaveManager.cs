using System;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public WaveGroupData waveGroupData;
    public GameManager gameManager;
    public Spawner spawner; // Inspector에서 연결 필요
    //public PoolSpawnerTest spawner; // Inspector에서 연결 필요

    private WaveEntry currentWaveData;
    private int currentWaveIndex = 0;
    private List<GameObject> spawnedMonsters = new List<GameObject>();

    [ContextMenu("스폰시키기")]
    public void StartWave()
    {
        // 기존 웨이브 목록 내에 있으면 복사해서 사용
        if (currentWaveIndex < waveGroupData.waveEntries.Count)
        {
            WaveEntry original = waveGroupData.waveEntries[currentWaveIndex];
            currentWaveData = new WaveEntry
            {
                overrideEnemies = original.overrideEnemies,
                wave = currentWaveIndex + 1
            };
        }
        else
        {
            currentWaveData = GenerateDynamicWaveEntry(currentWaveIndex + 1);
        }

        var spawnQueue = WaveBuilder.BuildWaveEntry(currentWaveData, waveGroupData.globalMonsterPool);
        SpawnMonsters(spawnQueue);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            DebugCurrentStatus();
        }
    }

    void DebugCurrentStatus()
    {
        Debug.Log($"[상태 확인]");

        if (currentWaveData == null)
        {
            Debug.LogWarning("currentWaveData가 설정되지 않았습니다.");
            return;
        }

        Debug.Log($"현재 웨이브: {currentWaveData.wave} (인덱스: {currentWaveIndex})");
        Debug.Log($"WaveValue: {currentWaveData.WaveValue}");
        Debug.Log($"ValueRange: ±{2 + (currentWaveData.wave / 10)}");
        Debug.Log($"살아있는 몬스터 수: {spawnedMonsters.Count}");

        if (gameManager != null)
        {
            Debug.Log($"보유 골드: {gameManager.GetGold()}");
            Debug.Log($"보유 쥬얼: {gameManager.GetJewel()}");
        }
    }

    void SpawnMonsters(List<MonsterData> monsters)
    {
        spawnedMonsters = spawner.SpawnMonsters(monsters, OnMonsterKilled);
    }

    void OnMonsterKilled(GameObject monster)
    {
        Debug.Log($"[몬스터 사망 감지] {monster.name} / ID: {monster.GetInstanceID()}");

        foreach (var m in spawnedMonsters)
        {
            Debug.Log($"[리스트에 있는 몬스터] {m.name} / ID: {m.GetInstanceID()}");
        }

        if (spawnedMonsters.Contains(monster))
        {
            spawnedMonsters.Remove(monster);
            Debug.Log($"[제거 성공] 남은 몬스터 수: {spawnedMonsters.Count}");
        }
        else
        {
            Debug.LogWarning($"[제거 실패] 리스트에 없는 몬스터: {monster.name}");
        }

        if (spawnedMonsters.Count == 0)
        {
            Debug.Log("[웨이브 클리어]");
            OnWaveCleared();
        }
    }

    void OnWaveCleared()
    {
        // 보상 지급
        gameManager.AddGold(currentWaveData.CalculateGoldReward());
        int? jewel = currentWaveData.TryGetJewelReward();
        if (jewel.HasValue)
            gameManager.AddJewel(jewel.Value);

        // 웨이브 인덱스 증가 후 다음 웨이브 시작
        currentWaveIndex++;
        StartWave();
    }

    private WaveEntry GenerateDynamicWaveEntry(int waveNumber)
    {
        return new WaveEntry
        {
            wave = waveNumber,
            overrideEnemies = new List<MonsterData>()
        };
    }
}
