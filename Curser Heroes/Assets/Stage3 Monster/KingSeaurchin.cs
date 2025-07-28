using UnityEngine;

public class KingSeaurchin : SeaurchinGroup
{
    public override void Setup(MonsterData data)
    {
        base.Setup(data);
        attackCooldown = 2f;  
    }
}
