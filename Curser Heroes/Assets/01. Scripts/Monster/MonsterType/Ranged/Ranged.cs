using UnityEngine;

public class Ranged : BaseMonster
{
    public GameObject projectilePrefab;    // 발사할 투사체 프리팹
    public Transform firePoint;             // 투사체 발사 위치 (Transform)
    public float attackRange = 5f;          // 공격 사거리

    // 공격 실행 함수 (BaseMonster 추상 메서드 구현)
    protected override void Attack()
    {
        // 공격 사거리 내 무기 오브젝트 탐색
        Collider2D weaponCollider = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Weapon"));

        if (weaponCollider != null)
        {
            // 무기 방향으로 투사체 발사
            Vector3 direction = (weaponCollider.transform.position - firePoint.position).normalized;

            if (projectilePrefab != null && firePoint != null)
            {
                // 투사체 생성
                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

                // 투사체 스크립트 초기화
                MonsterProjectile projScript = projectile.GetComponent<MonsterProjectile>();
                if (projScript != null)
                {
                    projScript.Initialize(direction, damage);
                }

                Debug.Log("원거리 공격: 투사체 발사!");
            }
        }
    }

    // 씬에서 공격 사거리 시각화 (디버그용)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
