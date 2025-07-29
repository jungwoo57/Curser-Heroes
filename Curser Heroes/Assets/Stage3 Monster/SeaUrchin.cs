using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class SeaUrchin : BaseMonster
{
   
    [SerializeField] private MonsterData monsterData;

    [Header("Attack Settings")]
    [SerializeField] private Vector2 attackOffset = new Vector2(0, 0.5f);
    [SerializeField] private float attackRange = 1.0f;
    [SerializeField] private LayerMask cursorLayer;
    [SerializeField] private float stunDuration = 1.0f;

    private void Awake()
    {
        
        var col = GetComponent<Collider2D>();
        col.isTrigger = false;
        var rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
    }

    protected override void Start()
    {
        base.Start();

        if (monsterData != null)
        {
            Setup(monsterData);
            Debug.Log($"[SeaUrchin] Setup: HP={CurrentHP}, CD={attackCooldown}");
        }
        else
        {
            Debug.LogWarning($"[{name}] MonsterData 할당 필요!");
        }

        attackTimer = attackCooldown;
    }

    protected override void Attack()
    {
        Vector2 origin = (Vector2)transform.position + attackOffset;
        Collider2D hit = Physics2D.OverlapCircle(origin, attackRange, cursorLayer);
        if (hit == null) return;

        if (hit.TryGetComponent<WeaponLife>(out var life))
            life.TakeLifeDamage();

        var mover = hit.GetComponent<CursorMoving>()
                    ?? hit.GetComponentInParent<CursorMoving>();
        if (mover != null) mover.Stun(stunDuration);
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<SubProjectile>(out var proj))
        {
            int dmg = proj.DamageAmount;
            Debug.Log($"[SeaUrchin] Hit by {proj.name}, dmg={dmg}");
            TakeDamage(dmg, proj.WeaponData);
            Destroy(proj.gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 origin = (Vector2)transform.position + attackOffset;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin, attackRange);
    }
}
