public class StunEffect : IEffect
{
    private float duration = 2f;
    private float timer = 0f;
    private Monster target;

    public void Apply(Monster target)
    {
        this.target = target;
     // target.Stun();
    }

    public void Tick(float deltaTime)
    {
        timer += deltaTime;
        if (IsFinished)
        {
        //  target.UnStun();
        }
    }

    public bool IsFinished => timer >= duration;
}
