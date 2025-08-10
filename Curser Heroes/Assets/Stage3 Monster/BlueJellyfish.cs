using UnityEngine;

public class BlueJellyfish : BaseMonster
{
    [SerializeField] private MonsterData monsterData;

    [Header("Cursor Attack Settings")]
    [SerializeField] private Vector2 attackOffset = new Vector2(0, 0.5f);
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private LayerMask cursorLayer;

    [Header("Explosion Settings")]
    [SerializeField] private Vector2 explosionOffset = new Vector2(0, 0.5f);
    [SerializeField] private float explosionRadius = 2f;

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
            Setup(monsterData);
        attackTimer = attackCooldown;
    }

    protected override void Attack()
    {
        Vector2 attackOrigin = (Vector2)transform.position + attackOffset;
        Collider2D hit = Physics2D.OverlapCircle(attackOrigin, attackRange, cursorLayer);

        if (hit != null && WeaponManager.Instance != null && !WeaponManager.Instance.isInvincible)
        {
            if (hit.gameObject.layer == LayerMask.NameToLayer("Weapon"))
            {
                WeaponManager.Instance.TakeWeaponLifeDamage();
            }
        }
    }

    protected override void Die()
    {
        Explode();
        base.Die();  
    }

    private void Explode()
    {
        Vector2 explodeOrigin = (Vector2)transform.position + explosionOffset;
        Collider2D[] hits = Physics2D.OverlapCircleAll(explodeOrigin, explosionRadius, cursorLayer);

        if (WeaponManager.Instance != null && !WeaponManager.Instance.isInvincible)
        {
            foreach (var hit in hits)
            {
                if (hit.gameObject.layer == LayerMask.NameToLayer("Weapon"))
                {
                    WeaponManager.Instance.TakeWeaponLifeDamage();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<SubProjectile>(out var proj))
        {
            int dmg = proj.DamageAmount;
            TakeDamage(dmg, proj.WeaponData);
            Destroy(proj.gameObject);
        }
    }


    private void OnDrawGizmosSelected()
    {
        
        Gizmos.color = Color.red;
        Vector2 attackOrigin = (Vector2)transform.position + attackOffset;
        Gizmos.DrawWireSphere(attackOrigin, attackRange);

       
        Gizmos.color = Color.yellow;
        Vector2 explodeOrigin = (Vector2)transform.position + explosionOffset;
        Gizmos.DrawWireSphere(explodeOrigin, explosionRadius);
    }
}
