using UnityEngine;

public class Ranged : BaseMonster
{
    public GameObject projectilePrefab; // 투사체 프리팹
    public Transform firePoint;         // 발사 위치
    public float attackRange = 5f;      // 공격 사거리
    public float spreadAngle = 15f;     // 좌우 퍼짐 각도

    protected override void Attack()
    {
        Collider2D weaponCollider = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Weapon"));
        if (weaponCollider == null) return;

        if (projectilePrefab != null && firePoint != null)
        {
            // 중심 방향 계산
            Vector3 direction = (weaponCollider.transform.position - firePoint.position).normalized;

            // 3개의 방향 생성: 중심, 좌측, 우측
            Vector3[] directions = new Vector3[3];
            directions[0] = direction; // 중심

            // 좌우 퍼짐 방향 생성 (Z축 회전)
            directions[1] = Quaternion.Euler(0, 0, spreadAngle) * direction; // 좌측
            directions[2] = Quaternion.Euler(0, 0, -spreadAngle) * direction; // 우측

            // 각 방향에 대해 투사체 생성
            foreach (Vector3 dir in directions)
            {
                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
                MonsterProjectile projScript = projectile.GetComponent<MonsterProjectile>();
                if (projScript != null)
                {
                    projScript.Initialize(dir, damage);
                }
            }

            Debug.Log("원거리 몬스터: 3갈래 투사체 발사!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
