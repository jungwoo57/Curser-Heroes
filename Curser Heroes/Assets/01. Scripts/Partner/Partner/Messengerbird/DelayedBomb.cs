using System.Collections;
using UnityEngine;

public class DelayedBomb : MonoBehaviour
{
    public float delay = 1f;
    public GameObject explosionEffect;
    private float damage;
    private float radius;

    public void Initialize(float damage, float radius)
    {
        this.damage = damage;
        this.radius = radius;
        StartCoroutine(ExplodeAfterDelay());
    }

    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        Debug.Log("[DelayedBomb] 폭발 발생");

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // 2D 탐지로 변경 (OverlapCircleAll)
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Monster"));
        Debug.Log($"검출된 콜라이더 수: {hits.Length}");

        foreach (Collider2D hit in hits)
        {
            Debug.Log($"감지된 몬스터: {hit.name}");
            BaseMonster enemy = hit.GetComponent<BaseMonster>();
            if (enemy != null)
            {
                enemy.TakeDamage((int)damage);
                Debug.Log($"[DelayedBomb] 피해 적용: {enemy.name} - {damage}");
            }
        }
        Destroy(gameObject);
    }
}