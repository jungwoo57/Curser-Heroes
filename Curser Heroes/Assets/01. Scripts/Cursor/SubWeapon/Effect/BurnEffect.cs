public class BurnEffect : IEffect
{
    private float duration = 5f;
    private float interval = 1f;
    private float timer = 0f;
    private float tickTimer = 0f;
    private int damagePerTick = 2;
    private Monster target;

    public void Apply(Monster target)
    {
        this.target = target;
    }

    public void Tick(float deltaTime)
    {
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
