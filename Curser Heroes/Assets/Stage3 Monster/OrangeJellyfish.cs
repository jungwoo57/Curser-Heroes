using UnityEngine;

public class OrangeJellyfish : BaseMonster
{
    [Header("Cursor Attack Settings")]
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private LayerMask cursorLayer;  

    protected override void Attack()
    {
        //  반경 안의 커서 오브젝트 감지
        Collider2D hit = Physics2D.OverlapCircle(transform.position, attackRange, cursorLayer);
        if (hit == null)
            return;

        
        var life = hit.GetComponent<WeaponLife>();
        if (life != null)
        {
            life.TakeLifeDamage();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
