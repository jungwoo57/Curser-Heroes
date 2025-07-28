using UnityEngine;

public class ArcaneTrail : MonoBehaviour
{
    public Animator animator;
    public float delayBeforeExplosion = 3f;
    public float radius = 1f;
    public LayerMask monsterLayer;
    public GameObject explosionEffect;

    private float timer = 0f;
    private int damage;
    private bool hasPlayedPending = false;

    public void Init(int damage)
    {
        this.damage = damage;
        animator.Play("Margin"); // 소환 직후 애니메이션
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!hasPlayedPending && timer >= 2f)
        {
            animator.Play("Pending"); // 폭발 직전 애니메이션
            hasPlayedPending = true;
        }

        if (timer >= delayBeforeExplosion)
        {
            Explode();
        }
    }

    void Explode()
    {
        // 폭발 이펙트
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // 피해 처리
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, monsterLayer);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<BaseMonster>(out var monster))
            {
                monster.TakeDamage(damage);
                continue;
            }

            // 보스 몬스터 처리
            if (hit.TryGetComponent<BossStats>(out var boss))
            {
                boss.TakeDamage(damage);
            }

        }

        Destroy(gameObject);
    }
}