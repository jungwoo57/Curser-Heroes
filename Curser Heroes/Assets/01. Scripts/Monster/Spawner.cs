using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float spawnRadius = 2.5f;  // 몬스터가 소환될 반경 범위

    // 몬스터 데이터 리스트를 받아서 각 몬스터를 겹치지 않는 랜덤 위치에 소환하고,
    // 몬스터가 죽을 때 호출될 콜백 함수도 연결해줌
    public List<GameObject> SpawnMonsters(List<MonsterData> monsters, System.Action<GameObject> onDeathCallback)
    {

        List<GameObject> spawned = new List<GameObject>();  // 실제 소환된 몬스터 오브젝트 리스트

        // 몬스터 수만큼 겹치지 않는 유니크한 위치를 생성
        List<Vector3> spawnPositions = UniquePositions(monsters.Count, spawnRadius);
        for (int i = 0; i < monsters.Count; i++)
        {
            MonsterData data = monsters[i];
            if (data == null || data.monsterPrefab == null) continue;  // 데이터나 프리팹 없으면 스킵

            Vector3 spawnPos = spawnPositions[i];  // 미리 생성한 위치 가져오기
            GameObject go = Instantiate(data.monsterPrefab, spawnPos, Quaternion.identity);  // 몬스터 생성
            go.transform.parent = gameObject.transform;
            go.transform.position = spawnPos;
            MonoBehaviour comp = go.GetComponent<MonoBehaviour>();

           
            if (comp is BaseMonster baseM)
            {
                baseM.Setup(data);
                baseM.onDeath += onDeathCallback;
            }
            spawned.Add(go);  // 리스트에 추가
        }

        return spawned;  // 생성된 몬스터 리스트 반환
    }

    // 지정된 반경 내에서 겹치지 않는 위치를 count 개수만큼 생성하는 함수
    private List<Vector3> UniquePositions(int count, float radius)
    {
        List<Vector3> positions = new List<Vector3>();
        int attempts = 0;  // 중복 위치 생성 방지용 시도 횟수 제한 변수

        // 원하는 위치 개수를 만들거나, 시도 횟수(count * 10)를 초과할 때까지 반복
        while (positions.Count < count && attempts < count * 10)
        {
            Vector3 candidate = GetSpawnPosition(radius);  // 랜덤 위치 생성
            bool tooClose = false;

            // 이미 생성된 위치들과 최소 1 유닛 이상 떨어져 있는지 검사
            foreach (var pos in positions)
            {
                if (Vector3.Distance(candidate, pos) < 1f)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)  // 충분히 떨어진 위치면 추가
            {
                positions.Add(candidate);
            }

            attempts++;  // 시도 횟수 증가
        }

        // 만약 충분한 위치를 못 만들었으면, 중복을 감수하고라도 랜덤 위치를 추가해서 개수 맞춤
        while (positions.Count < count)
        {
            positions.Add(GetSpawnPosition(radius));
        }

        return positions;  // 위치 리스트 반환
    }

    // 지정한 반경 내에서 위치 반환
    private static Vector3 GetSpawnPosition(float radius)
    {
        Vector2 randomPos = Random.insideUnitCircle * radius;
        return new Vector3(randomPos.x, randomPos.y, 0f);
    }
}
