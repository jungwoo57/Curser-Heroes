using UnityEngine;

public class LongRangeProjectile : SubProjectile
{
    void NewUpdate()
    {
        if (target == null || target.IsDead)
        {
            Destroy(gameObject);
            return;
        }

        // 빠르게 직선 이동
        Vector3 dir = (target.transform.position - transform.position).normalized;
        transform.position += dir * subweaponData.projectileSpeed * Time.deltaTime;

        // 충돌 처리
        if (Vector3.Distance(transform.position, target.transform.position) < 0.3f)
        {
            OnHit();
        }
    }
}
