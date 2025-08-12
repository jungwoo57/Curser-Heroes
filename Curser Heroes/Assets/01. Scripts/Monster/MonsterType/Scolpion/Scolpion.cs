
using UnityEngine;

public class Scolpion : BaseMonster
{
    public GameObject projectilePrefab; // 투사체 프리팹
    public Transform firePoint;         // 발사 위치
    public float attackRange = 5f;      // 공격 사거리
    [SerializeField] bool isAttacking = false;
    [SerializeField] private Vector2 targetPos;
    [SerializeField] private float maxDistance;
    [SerializeField] private float speed =5.0f;
    
    
    protected override void Attack()
    {
        Collider2D weaponCollider = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Weapon"));
        if (weaponCollider == null) return;
        
        targetPos = weaponCollider.transform.position; // 목표 지점 넣어주기
        maxDistance = Vector2.Distance(projectilePrefab.transform.position, targetPos);
        
        
        if (projectilePrefab != null && firePoint != null)
        {
            Vector3 direction = (weaponCollider.transform.position - firePoint.position).normalized;
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            ScolpionProjectile projScript = projectile.GetComponent<ScolpionProjectile>();
            if (projScript != null)
                projScript.Initialize(direction, damage, speed);
        }
        Debug.Log("원거리 몬스터: 투사체 발사!");
    }

    
    
    
}
