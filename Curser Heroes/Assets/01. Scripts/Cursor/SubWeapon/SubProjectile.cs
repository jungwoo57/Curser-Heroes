using UnityEngine;

public class SubProjectile : MonoBehaviour
{
    protected SubWeaponData subWeaponData;
    protected BaseMonster target;
    protected float speed;
    protected int calculatedDamage;  // Manager가 넘겨준 데미지 저장용

    // 기존 2-arg Init (하위 호환)
    public void Init(SubWeaponData data, BaseMonster tgt)
    {
        subWeaponData = data;
        target = tgt;
        speed = data.projectileSpeed;
        calculatedDamage = 0;
    }

    // 새 3-arg Init 오버로드: Manager에서 level·강화 반영된 데미지를 넘겨줌
    public void Init(SubWeaponData data, BaseMonster tgt, int damage)
    {
        Init(data, tgt);
        calculatedDamage = damage;
    }

    protected virtual void Update()
    {
        if (target == null || target.IsDead)
        {
            Destroy(gameObject);
            return;
        }

        // 타겟으로 이동
        Vector3 dir = (target.transform.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        // 충돌 판정
        if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
        {
            int dmg = (calculatedDamage > 0)
                      ? calculatedDamage
                      : Mathf.RoundToInt(subWeaponData.GetDamage());
            target.TakeDamage(dmg, subWeaponData);
            Destroy(gameObject);
        }
    }
}
