using UnityEngine;

public class ArcaneTrail : MonoBehaviour
{
    public Animator animator;
    public float delayBeforeExplosion = 3f;
    public float radius = 1.5f;
    public LayerMask monsterLayer;
    public GameObject explosionEffect;

    private float timer = 0f;
    private int damage;

    public void Init(int damage)
    {
        this.damage = damage;
        animator.Play("Margin"); // 처음 상태
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 2f && timer < 2.1f)
        {
            animator.Play("Pending"); // 2초 후 전조 애니메이션
        }

        if (timer >= delayBeforeExplosion)
        {
            Explode();
        }
    }

    void Explode()
    {
        // 연출
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // 피해 판정
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, monsterLayer);
        foreach (var hit in hits)
        {
            BaseMonster monster = hit.GetComponent<BaseMonster>();
            if (monster != null)
                monster.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
