using UnityEngine;

public class SeaUrchinKing   : SeaUrchin
{
    public override void Setup(MonsterData data)
    {
        base.Setup(data);
        attackCooldown = 2f;  
    }
}
