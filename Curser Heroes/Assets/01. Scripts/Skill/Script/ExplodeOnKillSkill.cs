using UnityEngine;

public class ExplodeOnKillSkill : MonoBehaviour
{
    public static void TriggerExplosion(Vector3 position, int damage, float radius, LayerMask monsterLayer)
    {
        // 이펙트 처리 (선택)
        // Instantiate(explosionVFXPrefab, position, Quaternion.identity);

        Collider2D[] hits = Physics2D.OverlapCircleAll(position, radius, monsterLayer);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out BaseMonster monster))
            {
                monster.TakeDamage(damage);
            }
        }

        Debug.Log($"[장렬한 퇴장] 폭발 발생! 위치: {position}, 피해량: {damage}");
    }
}
