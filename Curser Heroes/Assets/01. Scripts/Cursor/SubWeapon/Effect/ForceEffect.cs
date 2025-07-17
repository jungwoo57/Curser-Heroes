using UnityEngine;

public class ForceEffect : IEffect
{
    private Vector3 center;
    private int damage;
    private float radius;
    private LayerMask targetLayer;

    private float timer = 0f;
    private float duration = 0.1f;

    public ForceEffect(Vector3 center, int damage, float radius, LayerMask targetLayer)
    {
        this.center = center;
        this.damage = damage;
        this.radius = radius;
        this.targetLayer = targetLayer;
    }

    public void Apply(BaseMonster dummy)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius, targetLayer);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<BaseMonster>(out var monster) && !monster.IsDead)
            {
                monster.TakeDamage(damage);
            }
        }
    }

    public void Update(float deltaTime)
    {
        timer += deltaTime;
    }

    public bool IsFinished => timer >= duration;
}
