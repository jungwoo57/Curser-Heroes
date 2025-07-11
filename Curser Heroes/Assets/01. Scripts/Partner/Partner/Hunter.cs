using UnityEngine;

public class Hunter : BasePartner
{
    [Header("스킬 범위")]
    public float skillRange = 20f;

    protected override void ActivateSkill()
    {Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position,skillRange,LayerMask.GetMask("Monster"));

        //몬스터마다 1씩 피해 주기
        foreach (var hit in hits)
        {
            BaseMonster monster = hit.GetComponent<BaseMonster>();
            if (monster != null)
            {
                monster.TakeDamage(1, null);
                Debug.Log($"Hunter 스킬 발동: {monster.name}에게 1의 피해를 입힘");
            }
        }

        //스킬 사용 후 게이지 초기화
        currentGauge = 0f;
        ui.UpdateGauge(0f);
    }

}

