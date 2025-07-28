using UnityEngine;

public class SeaurchinGroup : BaseMonster
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 1.0f;
    [SerializeField] private LayerMask cursorLayer;

    protected override void Attack()
    {
        
        Collider2D hit = Physics2D.OverlapCircle(transform.position, attackRange, cursorLayer);
        if (hit == null) return;

        
        var life = hit.GetComponent<WeaponLife>();
        if (life != null)
            life.TakeLifeDamage();

        
        var mover = hit.GetComponent<CursorMoving>();
        if (mover != null)
            mover.Stun(1f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public override void Setup(MonsterData data)
    {
        base.Setup(data);
        attackCooldown = 1f; 
    }
}
