using UnityEngine;

public class FishLady : BaseMonster
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float attackRange = 5f;

    protected override void Attack()
    {
        Collider2D weaponCollider = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Weapon"));
        if (weaponCollider == null) return;

        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            HomingProjectile projScript = projectile.GetComponent<HomingProjectile>();
            if (projScript != null)
                projScript.Initialize(weaponCollider.transform, damage); // 🎯 무기 Transform 넘김
        }

        Debug.Log("원거리 몬스터: 유도 투사체 발사!");
    }
}
