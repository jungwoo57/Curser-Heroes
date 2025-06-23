using UnityEngine;

public class TestMonster : BaseMonster
{
    public GameObject fireballPrefab; // Inspector에서 지정
    public Transform firePoint;       // 발사 위치

    protected override void Attack()
    {
        if (fireballPrefab == null || firePoint == null) return;

        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);

        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = transform.right * 5f;
    }

    protected override void PlayHitEffect()
    {
        Debug.Log($"{gameObject.name} 피격 이펙트");
    }

    protected override void Die()
    {
        Debug.Log($"{gameObject.name} 사망");
        base.Die();
    }
}
