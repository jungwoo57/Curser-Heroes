using UnityEngine;
public class BurnEffect : IEffect
{
    private float duration = 5f;           // 총 지속 시간
    private float interval = 0.5f;           // 데미지 주기
    private float timer = 0f;
    private float tickTimer = 0f;
    private int damagePerTick = 2;
    private BaseMonster target;

    public void Apply(BaseMonster target)
    {
        this.target = target;
    }

    public void Update(float deltaTime)
    {
        if (target == null || target.IsDead) return;

        timer += deltaTime;
        tickTimer += deltaTime;

        if (tickTimer >= interval)
        {
            target.TakeDamage(damagePerTick);
            Debug.Log($"[BurnEffect] '{target.name}'가 화상으로 {damagePerTick} 데미지를 입었습니다. (총 경과: {timer:F2}s)");
            tickTimer = 0f;
        }
    }

    public bool IsFinished => timer >= duration;
}
