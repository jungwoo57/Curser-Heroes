using UnityEngine;

public class Spawner : MonoBehaviour
{
    //public MonsterData monsterData;  // Inspector에 할당
    //public float spawnRadius = 5f;   // 소환 반경

    //void Start()
    //{
    //    SpawnMonsterAtRandomPosition();
    //}

    //void SpawnMonsterAtRandomPosition()
    //{
    //    if (monsterData == null || monsterData.monsterPrefab == null) return;

    //    Vector3 spawnPos = SpawnMonster.GetSpawnPosition(spawnRadius); // 위치 계산
    //    GameObject monsterGO = Instantiate(monsterData.monsterPrefab, spawnPos, Quaternion.identity);

    //    BaseMonster monster = monsterGO.GetComponent<BaseMonster>();
    //    if (monster != null)
    //    {
    //        monster.maxHP = monsterData.maxHP;
    //        monster.valueCost = monsterData.valueCost;
    //        monster.attackCooldown = monsterData.attackCooldown;
    //    }
    //}
}
