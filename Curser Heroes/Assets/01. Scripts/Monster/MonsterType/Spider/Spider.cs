using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : BaseMonster
{
    public GameObject projectilePrefab; // 투사체 프리팹
    public Transform firePoint;         // 발사 위치
    public float attackRange = 5f;      // 공격 사거리
    protected override void Attack()
    {
        Collider2D weaponCollider = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Weapon"));
        if (weaponCollider == null) return;

        if (projectilePrefab != null && firePoint != null)
        {
            Vector3 direction = (weaponCollider.transform.position - firePoint.position).normalized;
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            MonsterProjectile projScript = projectile.GetComponent<MonsterProjectile>();
            if (projScript != null)
                projScript.Initialize(direction, damage);
        }


        Debug.Log("원거리 몬스터: 투사체 발사!");

    }
}
