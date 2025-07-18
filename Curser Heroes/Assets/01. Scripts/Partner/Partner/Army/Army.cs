using UnityEngine;

public class Army : BasePartner
{
    [Header("스킬 범위")]
    public float skillRange = 20f;

    [Header("이펙트 프리팹")]
    public GameObject armyEffectPrefab;  // ArmyEffect 프리팹 참조

    protected override void ActivateSkill()
    {
        // 1. 이펙트 생성 및 재생
        if (armyEffectPrefab != null)
        {
            GameObject effectObj = Instantiate(armyEffectPrefab, transform.position, Quaternion.identity);
            ArmyEffect effect = effectObj.GetComponent<ArmyEffect>();
            if (effect != null)
            {
                effect.PlayEffect(transform.position); // 중심 위치 전달
            }
        }

        // 2. 몬스터 피해 입히기
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, skillRange, LayerMask.GetMask("Monster"));
        foreach (var hit in hits)
        {
            BaseMonster monster = hit.GetComponent<BaseMonster>();
            if (monster != null)
            {
                monster.TakeDamage(1, null);
                Debug.Log($"Army 스킬 발동: {monster.name}에게 1의 피해를 입힘");
            }
        }

        // 3. 스킬 게이지 초기화
        currentGauge = 0f;
        ui.UpdateGauge(0f);
    }
}