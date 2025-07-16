using UnityEngine;

public class ShortRangeProjectile : SubProjectile
{
    private float lifeTime = 1.5f;
    private float timer = 0f;

     void newUpdate()
    {
        timer += Time.deltaTime;

        if (target == null || target.IsDead || timer > lifeTime)
        {
            Destroy(gameObject);
            return;
        }

        // 느리게 이동 (단거리용)
        Vector3 dir = (target.transform.position - transform.position).normalized;
        transform.position += dir * (subweaponData.projectileSpeed * 0.5f) * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.transform.position) < 0.25f)
        {
            OnHit();
        }
    }
}
