using UnityEngine;

public class SeaurchinGroup : BaseMonster
{
    [Header("Attack Settings")]
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
        Debug.Log($"[{name}]  Attack() at {Time.time:F2}");

        
        Collider2D hit = Physics2D.OverlapCircle(transform.position, attackRange, cursorLayer);
        if (hit == null)
        {
            Debug.Log($"[{name}] └ No cursor Collider in range {attackRange}");
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
