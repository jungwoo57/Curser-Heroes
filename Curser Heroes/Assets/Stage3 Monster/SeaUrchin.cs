using UnityEngine;
using UnityEngine.Timeline;

public class SeaUrchin : BaseMonster
{
    [Header("Attack Settings")]
    [SerializeField] private Vector2 attackOffset = new Vector2(0, 0.5f);
    [SerializeField] private float attackRange = 1.0f;
    [SerializeField] private LayerMask cursorLayer;
    [SerializeField] private float stunDuration = 1.0f;

    protected override void Start()
    {
        base.Start();
        attackCooldown = 1f;
        attackTimer = attackCooldown;
    }

    protected override void Attack()
    {
        Vector2 origin = (Vector2)transform.position + attackOffset;
        Collider2D hit = Physics2D.OverlapCircle(origin, attackRange, cursorLayer);


        
        if (hit == null)
        {
            
            return;
        }

        Debug.Log($"[{name}]  Detected {hit.name} in range");

        
        var life = hit.GetComponent<WeaponLife>();
        if (life != null)
        {
           
            life.TakeLifeDamage();
        }
        else
        {
            
        }

        
        var mover = hit.GetComponent<CursorMoving>() ?? hit.GetComponentInParent<CursorMoving>();
        if (mover != null)
        {
           
            mover.Stun(stunDuration);
        }
        else
        {
           
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 origin = (Vector2)transform.position + attackOffset;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin, attackRange);
    }
}
