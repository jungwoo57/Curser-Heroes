using UnityEngine;

public class SpawnMonster : MonoBehaviour
{
    // 전역 접근 가능한 랜덤 위치 반환 함수
    public static Vector3 GetSpawnPosition(float radius = 5f)
    {
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        return new Vector3(randomDir.x, randomDir.y, 0f) * radius;
    }
}
