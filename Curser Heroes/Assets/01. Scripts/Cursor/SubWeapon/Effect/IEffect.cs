public interface IEffect
{
    void Apply(Monster target);
    void Tick(float deltaTime);
    bool IsFinished { get; }
}
