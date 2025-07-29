using UnityEngine;

public class SeaUrchinLady : SeaUrchin
{
    public override void Setup(MonsterData data)
    {
        base.Setup(data);
        attackCooldown = 1f;  
    }
}
