using System.Collections;
using System.Collections.Generic;
using System.Threading;
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

        List<MonsterData> spawnQueue = WaveBuilder.BuildWave(waveData, allMonsters);  //소환할 몬스터 리스트 생성

        SpawnMonsters(spawnQueue); //생성된 리스트를 기반으로 몬스터소환
    }
    void SpawnMonsters(List<MonsterData> monsters)
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            Vector3 spawnPos = spawnPoints[i % spawnPoints.Length].position;
            GameObject go = Instantiate(monsters[i].prefab, spawnPos, Quaternion.identity);

            Monster monster = go.GetComponent<Monster>();
            if (monster != null)
            {
                monster.Setup(monsters[i]);
                monster.onDeath += OnMonsterKilled;  //몬스터가 죽었을 때 호출될 이벤트 등록
            }
            spawnedMonsters.Add(go); //몬스터 소환 확인용 코드
        }
    }

    void OnMonsterKilled(GameObject monster) // 몬스터 죽음 확인 부분
    {
        spawnedMonsters.Remove(monster);
        if (spawnedMonsters.Count == 0)
        {
            OnWaveCleared();
        }
    }

    void OnWaveCleared() //보상 지급 및 웨이브 이동
    {
        WaveData waveData = waveList[currentWaveIndex];

        gameManager.AddGold(waveData.CalculateGoldReward());

        int? jewel = waveData.TryGetJewelReward();
        if (jewel.HasValue)
            gameManager.AddJewel(jewel.Value);

        currentWaveIndex++;
    }
}