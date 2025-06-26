using UnityEngine;

public class Ranged : BaseMonster
{
    public GameObject projectilePrefab; // 투사체 프리팹
    public Transform firePoint;         // 발사 위치
    public float attackRange = 5f;      // 공격 사거리

    protected override void Attack()
    {
        // 무기가 사거리 내에 있는지 확인
        Collider2D weaponCollider = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Weapon"));
        if (weaponCollider == null) return;

        if (projectilePrefab != null && firePoint != null)
        {
            // 무기 방향 계산
            Vector3 direction = (weaponCollider.transform.position - firePoint.position).normalized;

            // 투사체 생성
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            // 투사체 초기화
            MonsterProjectile projScript = projectile.GetComponent<MonsterProjectile>();
            if (projScript != null)
            {
                projScript.Initialize(direction, damage);
            }

            Debug.Log("원거리 몬스터: 투사체 발사!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
