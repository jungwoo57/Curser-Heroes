using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<MonsterData> allMonsters;
    public List<WaveData> waveList;
    public Transform[] spawnPoints;
    public GameManager gameManager;

    private int currentWaveIndex = 0;
    private List<GameObject> spawnedMonsters = new List<GameObject>();

    public void StartWave()
    {
        if (currentWaveIndex >= waveList.Count)
        {
            Debug.Log("모든 웨이브 클리어!");
            return;
        }

        WaveData waveData = waveList[currentWaveIndex];

        List<MonsterData> spawnQueue = WaveBuilder.BuildWave(waveData, allMonsters);
    }
    void OnWaveCleared()
    {
        WaveData waveData = waveList[currentWaveIndex];

        gameManager.AddGold(waveData.CalculateGoldReward());

        int? jewel = waveData.TryGetJewelReward();
        if (jewel.HasValue)
            gameManager.AddJewel(jewel.Value);

        currentWaveIndex++;
    }
}