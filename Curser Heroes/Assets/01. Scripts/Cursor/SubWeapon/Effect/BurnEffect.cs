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
            tickTimer = 0f;
        }
    }

    public bool IsFinished => timer >= duration;
}
