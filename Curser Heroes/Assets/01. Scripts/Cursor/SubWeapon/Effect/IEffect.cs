public interface IEffect
{
    void Apply(BaseMonster target);         // 어떤 대상에게 적용되는지
    void Update(float deltaTime);          // 시간 경과용
    bool IsFinished { get; }               // 완료 여부
}
