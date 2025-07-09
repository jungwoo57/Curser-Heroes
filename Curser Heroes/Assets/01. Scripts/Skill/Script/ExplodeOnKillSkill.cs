using UnityEngine;

public class ExplodeOnKillSkill : MonoBehaviour
{
    public static GameObject explosionPrefab;
    public  void TriggerExplosion(Vector3 position, int damage, float radius, LayerMask monsterLayer)
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
        Debug.Log($"[장렬한 퇴장] {hits.Length}개의 오브젝트가 범위 안에 감지됨");

        foreach (var hit in hits)
        {
            Debug.Log($"▶ 감지된 오브젝트: {hit.name}, Tag: {hit.tag}, Layer: {LayerMask.LayerToName(hit.gameObject.layer)}");

            if (hit.TryGetComponent(out BaseMonster monster))
            {
                Debug.Log($"✔ {monster.name} 은(BaseMonster) 맞음! 데미지 입힘");
                monster.TakeDamage(damage);
            }
        }
    }
}
