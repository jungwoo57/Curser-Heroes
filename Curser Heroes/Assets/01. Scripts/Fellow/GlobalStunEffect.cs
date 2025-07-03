public class GlobalStunEffect : IFellowSpecialEffect
{
    public void Execute(float duration)
    {
        EnemyManager.Instance?.StunAllEnemies(duration);
    }
}
