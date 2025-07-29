using UnityEngine;

public class IceAgeField : MonoBehaviour
{
    private float duration = 4f;
    private float tickInterval = 1f;
    private float tickTimer = 0f;
    private float lifeTimer = 0f;
    private int damage;

    public void Setup(int damage)
    {
        this.damage = damage;
    }

    void Update()
    {
        lifeTimer += Time.deltaTime;
        tickTimer += Time.deltaTime;

        if (tickTimer >= tickInterval && lifeTimer < duration)
        {
            tickTimer = 0f;
            DealDamage();
        }

        if (lifeTimer >= duration)
        {
            Destroy(gameObject);
        }
    }

    private void DealDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1f, LayerMask.GetMask("Monster"));
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<BaseMonster>(out var monster))
            {
                monster.TakeDamage(damage);
                continue;
            }

            if (hit.TryGetComponent<BossStats>(out var boss))
            {
                boss.TakeDamage(damage);
            }
        }
    }
}