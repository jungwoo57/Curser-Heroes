public interface IEffect
{
    void Apply(BaseMonster monster);
    void Update(float deltaTime);
    bool IsFinished { get; }
}