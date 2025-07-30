using UnityEngine;

public class KingSeaUrchin : SeaUrchin
{
    public override void Setup(MonsterData data)
    {
        base.Setup(data);
        attackCooldown = 2f;  
    }
}
