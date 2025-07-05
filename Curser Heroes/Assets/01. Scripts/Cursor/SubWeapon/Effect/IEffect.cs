public interface IEffect
{
    void Apply(BaseMonster target);
    void Update(float deltaTime);
    bool IsFinished { get; }
}
