using UnityEngine;

public class ForceEffect : IEffect
{
    private float radius;
    private int damage;
    private float timer;
    private float duration = 0.1f;
    private LayerMask targetLayer;

    private Vector3 center;

    public ForceEffect(Vector3 center, int damage, float radius, LayerMask targetLayer)
    {
        this.center = center;
        this.damage = damage;
        this.radius = radius;
        this.targetLayer = targetLayer;
    }

    public void Apply(BaseMonster dummy)  // 
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
