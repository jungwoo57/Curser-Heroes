using UnityEngine;

public class LongRangeProjectile : SubProjectile
{
    [Header("추가 효과 (스턴)")]
    [Tooltip("명중 시 이 투사체가 스턴을 걸 것인지")]
    public bool applyStun = false;

    [Tooltip("스턴 지속 시간 (초)")]
    public float stunDuration = 1f;

    [Header("추적 속도")]
    public float extraSpeedMultiplier = 1.5f;

    // 부모(SubProjectile)가 virtual로 정의해 둔 Update를 override
    protected override void Update()
    {
        if (target == null || target.IsDead)
        {
            Destroy(gameObject);
            return;
        }

        // 빠르게 직선 이동
        Vector3 dir = (target.transform.position - transform.position).normalized;
        float speed = subweaponData.projectileSpeed * extraSpeedMultiplier;
        transform.position += dir * speed * Time.deltaTime;

        // 충돌 처리: 적과 가까워지면 OnHit 호출
        if (Vector3.Distance(transform.position, target.transform.position) < 0.3f)
        {
            OnHit();
        }
    }

    // 충돌 시 부모 로직 실행 후 스턴을 걸어 줍니다.
    protected override void OnHit()
    {
        // 데미지 및 이펙트 적용 
        base.OnHit();

        // Inspector에서 따로 지정 가능
        if (applyStun && stunDuration > 0f && target != null && !target.IsDead)
        {
            var em = target.GetComponent<EffectManager>();
            if (em != null)
            {
               // em.AddEffect(new StunEffect(target, stunDuration));
            }
        }

       
    }
}
