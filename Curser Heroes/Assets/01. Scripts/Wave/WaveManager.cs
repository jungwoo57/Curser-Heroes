
using System;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private SkillManager skillManager;

    public WaveGroupData waveGroupData;
    public GameManager gameManager;
    public Spawner spawner;
    
    //public PoolSpawnerTest spawner; // Inspector에서 연결 필요

    private WaveEntry currentWaveData;
    private int currentWaveIndex = 0;
    private List<GameObject> spawnedMonsters = new List<GameObject>();
    private bool waveCleared = false; // 중복 웨이브 클리어 방지

    [ContextMenu("스폰시키기")]
    public void StartWave()
    {
        waveCleared = false; // 새 웨이브 시작 시 초기화

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

        Debug.Log($"웨이브 시작: {currentWaveData.wave} (인덱스: {currentWaveIndex})");

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

        GameObject toRemove = null;

        foreach (var m in spawnedMonsters)
        {
            if (m == monster) // 참조 직접 비교
            {
                toRemove = m;
                break;
            }
        }

        if (toRemove != null)
        {
            spawnedMonsters.Remove(toRemove);
            Debug.Log($"[제거 성공] 남은 몬스터 수: {spawnedMonsters.Count}");
        }
        else
        {
            Debug.LogWarning($"[제거 실패] 리스트에 없는 몬스터: {monster.name}");
        }

        if (!waveCleared && spawnedMonsters.Count == 0)
        {
            waveCleared = true; // 중복 방지
            Debug.Log("[웨이브 클리어]");
            OnWaveCleared();
        }
    }

    void OnWaveCleared()
    {
        gameManager.AddGold(currentWaveData.CalculateGoldReward());
        int? jewel = currentWaveData.TryGetJewelReward();
        if (jewel.HasValue)
            gameManager.AddJewel(jewel.Value);

        skillManager.OnWaveEnd(); // → 내부에서 스킬 또는 보상 선택 UI 표시
    }

    public void IncrementWaveIndex()
    {
        currentWaveIndex++;
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
