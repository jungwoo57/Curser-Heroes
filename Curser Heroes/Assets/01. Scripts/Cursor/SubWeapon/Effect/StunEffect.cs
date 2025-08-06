public class StunEffect : IEffect
{
    private readonly float duration;
    private float timer;

    public StunEffect(float duration)
    {
        this.duration = duration;
    }

    public void Apply(BaseMonster monster)
    {
        //  몬스터 로직으로 스턴
        monster.Stun(duration);

        //  몬스터 위에 스턴 아이콘 띄우기
        if (DamageTextManager.instance != null)
            DamageTextManager.instance.ShowStun(monster.transform, duration);
    }

    public void Update(float deltaTime)
    {
        timer += deltaTime;
    }

    public bool IsFinished => timer >= duration;
}
