using UnityEngine;

public class ExplodingBomb : MonoBehaviour
{
    private int damage;
    private float radius;
    private LayerMask monsterLayer;
    private float delay = 1f;

    public void Init(int damage, float radius, LayerMask monsterLayer)
    {
        this.damage = damage;
        this.radius = radius;
        this.monsterLayer = monsterLayer;
        Invoke(nameof(Explode), delay);
    }

    private void Explode()
    {
        // 이펙트 생성
        if (ExplodeOnKillSkill.explosionPrefab != null)
        {
            Instantiate(ExplodeOnKillSkill.explosionPrefab, transform.position, Quaternion.identity);
            Debug.Log("[장렬한 퇴장] 폭탄이 폭발함!");
        }

        // 데미지 처리
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, monsterLayer);
        Debug.Log($"[장렬한 퇴장] 폭발 범위 내 {hits.Length}개의 적 감지됨");

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out BaseMonster monster))
            {
                Debug.Log($"→ {monster.name} 데미지 {damage} 입힘");
                monster.TakeDamage(damage);
            }
        }

        Destroy(gameObject); // 폭탄 제거
    }
}