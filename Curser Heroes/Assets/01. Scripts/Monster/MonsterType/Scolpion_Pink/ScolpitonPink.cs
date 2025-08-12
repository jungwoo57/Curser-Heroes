using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScolpitonPink : BaseMonster
{
    public GameObject projectilePrefab; // 투사체 프리팹
    public Transform firePoint;         // 발사 위치
    public float attackRange = 5f;      // 공격 사거리

    [SerializeField] int count = 3;   // 공격 갯수
    [SerializeField] private Vector2 targetPos;
    [SerializeField] private float speed =5.0f;
    [SerializeField] private float angle = 20.0f;
    
    protected override void Attack()
    {
        Collider2D weaponCollider = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Weapon"));
        if (weaponCollider == null) return;
        
        targetPos = weaponCollider.transform.position; // 목표 지점 넣어주기
        
        
        if (projectilePrefab != null && firePoint != null)
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 direction = (weaponCollider.transform.position - firePoint.position).normalized;
                Vector3 newDir =  Quaternion.Euler(0,0, -angle + angle*i)* direction;
                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
                ScolpionProjectile projScript = projectile.GetComponent<ScolpionProjectile>();
                if (projScript != null)
                    projScript.Initialize(newDir, damage, speed);
            }
        }
        Debug.Log("원거리 몬스터: 투사체 발사!");
    }
    
}
