public class StunEffect : IEffect
{
    private readonly float duration;
    private float timer;

    public StunEffect(float duration)
    {
        this.duration = duration;
    }

    // Apply 시점에 몬스터.Stun(duration) 호출
    public void Apply(BaseMonster monster)
    {
        monster.Stun(duration);
    }

    public void Update(float deltaTime)
    {
        timer += deltaTime;
    }

    public bool IsFinished => timer >= duration;
}
