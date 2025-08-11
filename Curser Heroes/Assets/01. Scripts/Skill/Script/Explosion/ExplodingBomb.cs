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
            AudioSource audioSource = ExplodeOnKillSkill.explosionPrefab.GetComponent<AudioSource>();
            if (audioSource != null && audioSource.clip != null)
            {
                // PlayClipAtPoint에 볼륨 인자를 추가하여 볼륨 조절
                // 0.7f는 예시이며, 배경음악에 맞춰 적절한 값을 사용하세요.
                AudioSource.PlayClipAtPoint(audioSource.clip, transform.position, 1f);
                Debug.Log("[장렬한 퇴장] 폭탄 폭발음 재생!");
            }

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