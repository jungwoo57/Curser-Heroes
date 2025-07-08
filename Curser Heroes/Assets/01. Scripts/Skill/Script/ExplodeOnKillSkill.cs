using UnityEngine;

public class ExplodeOnKillSkill : MonoBehaviour
{
    public static GameObject explosionPrefab;
    public static void TriggerExplosion(Vector3 position, int damage, float radius, LayerMask monsterLayer)
    {
        if (explosionPrefab != null)
        {
            GameObject vfx = GameObject.Instantiate(explosionPrefab, position, Quaternion.identity);
            Debug.Log($"[장렬한 퇴장] 폭발 이펙트 생성됨: {vfx.name}");
        }
        else
        {
            Debug.LogWarning("explosionVFXPrefab이 null입니다!");
        }
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
