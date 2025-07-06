using UnityEngine;

public abstract class SubProjectile : MonoBehaviour
{
    protected SubWeaponData subweaponData;
    protected BaseMonster target;

    public virtual void Init(SubWeaponData data, BaseMonster targetMonster)  // 
    {
        subweaponData = data;
        target = targetMonster;
    }
    protected virtual void Update()
    {
        if (target == null || target.IsDead)     //몬스터가 null이거나 죽었으면 투사체 삭제
        {
            Destroy(gameObject);
            return;
        }

        
        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.position += direction * subweaponData.projectileSpeed * Time.deltaTime;  //projectileSpeed를 사용하여 타겟 직선 이동

        // 거리 체크 후 충돌 처리
        if (Vector3.Distance(transform.position, target.transform.position) < 0.3f)   //거리가 0.3 이하면 충돌로 간주
        {
            ApplyDamage(target);
            ApplyEffect(target);
            Destroy(gameObject);                 //데미지, 이펙트 적용 후 투사체 파괴
        }
    }



    protected void ApplyDamage(BaseMonster monster)
    {
        int dmg = Mathf.RoundToInt(subweaponData.GetDamage());
        Debug.Log($"[SubProjectile] {monster.gameObject.name} 에게 {dmg} 데미지!");
        target.TakeDamage(dmg, subweaponData);     //서브웨폰 데이터에 들어있는 공격력을 베이스 몬스터의
                                                //TakeDamage로 전달
    }

    [SerializeField] private GameObject effectManagerPrefab;   //이펙트 매니저 프리팹 연결

    protected void ApplyEffect(BaseMonster monster)
    {
        if (monster.TryGetComponent(out EffectManager effectManager))     //몬스터가 이펙트매니저를 갖고 있으면 효과 적용
        {
            IEffect effect = EffectFactory.CreateEffect(subweaponData.effect);
            if (effect != null)
                effectManager.AddEffect(effect);
        }
    }

    protected virtual void OnHit()
    {
        ApplyDamage(target);
        ApplyEffect(target);
        Destroy(gameObject);       //데미지 적용,이펙트 적용, 삭제
    }

}
