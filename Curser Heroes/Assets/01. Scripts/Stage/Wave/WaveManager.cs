using System;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<MonsterData> allMonsters;
    public WaveGroupData waveGroupData;
    public GameManager gameManager;
    public Spawner spawner; // Inspector에서 연결 필요
    //public PoolSpawnerTest spawner; // Inspector에서 연결 필요
    //public PoolRefactoryTestSpawn spawner;


    private int currentWaveIndex = 0;
    private List<GameObject> spawnedMonsters = new List<GameObject>();

    public void StartWave()
    {
        WaveEntry waveData;

        // 기존 WaveEntry 리스트 안에 있을 경우
        if (currentWaveIndex < waveGroupData.waveEntries.Count)
        {
            waveData = waveGroupData.waveEntries[currentWaveIndex];
        }
        else
        {
            // 무한 웨이브용 임시 WaveEntry 생성
            waveData = GenerateDynamicWaveEntry(currentWaveIndex + 1);
        }
        List<MonsterData> spawnQueue = WaveBuilder.BuildWaveEntry(waveData, waveGroupData.globalMonsterPool);  //소환할 몬스터 리스트 생성

        SpawnMonsters(spawnQueue); //생성된 리스트를 기반으로 몬스터소환
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) // 예: D 키를 눌렀을 때 출력
        {
            DebugCurrentStatus();
        }
    }

    void DebugCurrentStatus()
    {
        Debug.Log($"[🔍 상태 확인]");
        int totalWave = Mathf.Max(currentWaveIndex + 1, waveGroupData.waveEntries.Count);
        int displayWave = currentWaveIndex + 1;
        Debug.Log($"현재 웨이브: {currentWaveIndex + 1} / 총 웨이브: {totalWave}");

        WaveEntry waveData = (currentWaveIndex < waveGroupData.waveEntries.Count)
        ? waveGroupData.waveEntries[currentWaveIndex]
        : GenerateDynamicWaveEntry(displayWave);

        int waveValue = waveData.WaveValue;
        int valueRange = 2 + (waveData.wave / 10);

        Debug.Log($"🧮 WaveValue: {waveValue}");
        Debug.Log($"📈 ValueRange: ±{valueRange}");

        Debug.Log($"살아있는 몬스터 수: {spawnedMonsters.Count}");

        if (gameManager != null)
        {
            Debug.Log($"보유 골드: {gameManager.GetGold()}");
            Debug.Log($"보유 쥬얼: {gameManager.GetJewel()}");
        }
        else
        {
            Debug.LogWarning("GameManager가 연결되어 있지 않습니다.");
        }
    }

    void SpawnMonsters(List<MonsterData> monsters)
    {
        spawnedMonsters = spawner.SpawnMonsters(monsters, OnMonsterKilled);
    }

    void OnMonsterKilled(GameObject monster) // 몬스터 죽음 확인 부분
    {
        spawnedMonsters.Remove(monster);
        if (spawnedMonsters.Count == 0)
        {
            OnWaveCleared();
        }
    }

    void OnWaveCleared()
    {
        WaveEntry waveData = currentWaveIndex < waveGroupData.waveEntries.Count ? waveGroupData.waveEntries[currentWaveIndex]:GenerateDynamicWaveEntry(currentWaveIndex + 1);

        gameManager.AddGold(waveData.CalculateGoldReward());

        int? jewel = waveData.TryGetJewelReward();
        if (jewel.HasValue)
            gameManager.AddJewel(jewel.Value);

        currentWaveIndex++;
    }
    private WaveEntry GenerateDynamicWaveEntry(int waveNumber)
    {
        WaveEntry newWave = new WaveEntry
        {
            wave = waveNumber,
            overrideEnemies = new List<MonsterData>() // 비워두면 globalMonsterPool 사용됨
        };

        return newWave;
    }
   
}
