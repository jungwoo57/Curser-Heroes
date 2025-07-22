using UnityEngine;

public class LongRangeProjectile : SubProjectile
{
    [Header("Stun")]
    public bool applyStun = false;
    public float stunDuration = 1f;

    [Header("속도 배율")]
    public float extraSpeedMultiplier = 1.5f;

    [Header("Explosion VFX")]
    [Tooltip("충돌 후 터지는 애니메이션용 프리팹")]
    public GameObject explosionPrefab;
    [Tooltip("VFX가 재생된 후 사라질 시간")]
    public float explosionLifetime = 1f;

    protected override void Update()
    {
        if (target == null || target.IsDead)
        {
            Destroy(gameObject);
            return;
        }

       
        Vector3 targetPos = target.transform.position;
        var sr = target.GetComponent<SpriteRenderer>();
        if (sr != null) targetPos = sr.bounds.center;
        targetPos.z = transform.position.z;

        
        Vector3 dir = (targetPos - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

       
        float curSpeed = subWeaponData.projectileSpeed * extraSpeedMultiplier;
        transform.position += dir * curSpeed * Time.deltaTime;

       
        if (Vector3.Distance(transform.position, targetPos) < 0.3f)
            OnHit();
    }
    private void OnHit()
    {
       
        int dmg = (calculatedDamage > 0)
                  ? calculatedDamage
                  : Mathf.RoundToInt(subWeaponData.GetDamage());
        target.TakeDamage(dmg, subWeaponData);
        if (applyStun) target.Stun(stunDuration);

        
        if (explosionPrefab != null)
        {
            var fx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(fx, explosionLifetime);
        }

        
        Destroy(gameObject);
    }
}
