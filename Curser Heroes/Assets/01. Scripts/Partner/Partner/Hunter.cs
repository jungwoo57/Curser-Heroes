using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hunter : BasePartner
{
    public float skillRange = 20f;
    BaseMonster baseMonster;
    private void Update()
    {
        if(currentGauge >= data.gaugeMax)
        {
            ActivateSkill();
        }
    }
    protected override void ActivateSkill()
    {
        Collider2D monsterCollider = Physics2D.OverlapCircle(transform.position, skillRange, LayerMask.GetMask("Monster"));
        if (monsterCollider != null)
        {
            if (baseMonster != null)
            {
                baseMonster.TakeDamage(1,null);
                Debug.Log($"Hunter 스킬 발동: {baseMonster.name}에게 1의 피해를 입혔습니다.");
            }
            else
            {
                Debug.LogWarning("monsterCollider를 찾을 수 없습니다!");
            }
        }
    }

}
