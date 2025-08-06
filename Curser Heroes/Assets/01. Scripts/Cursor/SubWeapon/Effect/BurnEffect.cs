public class BurnEffect : IEffect
{
    private readonly BaseMonster target;
    private readonly float duration;
    private readonly float interval;
    private readonly int damagePerTick;
    private float timer;
    private float tickTimer;

    public BurnEffect(BaseMonster target, int damagePerSecond, float durationSeconds, float tickInterval = 1f)
    {
        this.target = target;
        this.damagePerTick = damagePerSecond;
        this.duration = durationSeconds;
        this.interval = tickInterval;
        this.timer = 0f;
        this.tickTimer = 0f;
    }

    // ★ 이 부분을 채워주세요
    public void Apply(BaseMonster _)
    {
        // 몬스터 위에 화상 아이콘 띄우기
        if (DamageTextManager.instance != null)
            DamageTextManager.instance.ShowBurn(target.transform, duration);
    }

    public void Update(float deltaTime)
    {
        if (target == null || target.IsDead) return;
        timer += deltaTime;
        tickTimer += deltaTime;

        if (tickTimer >= interval)
        {
            target.TakeDamage(damagePerTick);
            tickTimer -= interval;
        }
    }

    public bool IsFinished => timer >= duration;
}
