using UnityEngine;

public class BurnEffect : IEffect
{
    private readonly BaseMonster target;
    private readonly float duration;
    private readonly float interval;
    private readonly int damagePerTick;
    private float timer;
    private float tickTimer;

    // 생성자에서 대상, 초당 데미지, 지속시간, 틱 간격을 모두 받습니다.
    public BurnEffect(BaseMonster target, int damagePerSecond, float durationSeconds, float tickInterval = 1f)
    {
        this.target = target;
        this.damagePerTick = damagePerSecond;
        this.duration = durationSeconds;
        this.interval = tickInterval;
        this.timer = 0f;
        this.tickTimer = 0f;
        Debug.Log($"[BurnEffect] Created on '{target.name}' → {damagePerTick}/sec for {duration}sec, tick every {interval}sec");
    }

    // IEffect 요구 메서드: Apply는 빈 구현
    public void Apply(BaseMonster _)
    {
    }

    // 매 프레임 호출됩니다.
    public void Update(float deltaTime)
    {
        if (target == null || target.IsDead) return;

        timer += deltaTime;
        tickTimer += deltaTime;

        Debug.Log($"[BurnEffect] Update on '{target.name}': timer={timer:F2}s, tickTimer={tickTimer:F2}s");

        if (tickTimer >= interval)
        {
            Debug.Log($"[BurnEffect] '{target.name}' takes {damagePerTick} burn damage");
            target.TakeDamage(damagePerTick);
            tickTimer -= interval;
        }
    }

    public bool IsFinished => timer >= duration;
}
