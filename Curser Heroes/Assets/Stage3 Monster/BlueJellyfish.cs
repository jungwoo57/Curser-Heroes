using UnityEngine;

public class Blueparipari : BaseMonster
{
    [Header("Cursor Attack Settings")]
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private LayerMask cursorLayer;

    [Header("Explosion Settings")]
    [SerializeField] private float explosionRadius = 2f;
    

    protected override void Attack()
    {
        
        Collider2D hit = Physics2D.OverlapCircle(transform.position, attackRange, cursorLayer);
        if (hit != null)
        {
            var life = hit.GetComponent<WeaponLife>();
            if (life != null)
                life.TakeLifeDamage();
        }
    }

    protected override void Die()
    {
        Explode();
        base.Die();  // BaseMonster.Die() 호출 (애니메이션, 파괴 등)
    }

    private void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, cursorLayer);
        foreach (var hit in hits)
        {
            var life = hit.GetComponent<WeaponLife>();
            if (life != null)
                life.TakeLifeDamage();
        }
    }

    private void OnDrawGizmosSelected()
    {
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
