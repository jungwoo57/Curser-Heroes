public class StunEffect : IEffect
{
    private float duration = 2f;
    private float timer = 0f;
    private BaseMonster target;
    private bool hasStunned = false;

    public void Apply(BaseMonster target)
    {
        this.target = target;
        if (target != null && !target.IsDead)
        {
            //target.Stun();   
            hasStunned = true;
        }
    }

    public void Update(float deltaTime)
    {
        if (target == null || target.IsDead) return;

        timer += deltaTime;

        if (IsFinished && hasStunned)
        {
            target.UnStun();   
            hasStunned = false;
        }
    }

    public bool IsFinished => timer >= duration;
}
