using UnityEngine;

public class SeaurchinLamb : SeaurchinGroup
{
    public override void Setup(MonsterData data)
    {
        base.Setup(data);
        attackCooldown = 1f;  
    }
}
