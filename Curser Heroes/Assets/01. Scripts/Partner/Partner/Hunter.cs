using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hunter : BasePartner
{
    public float skillRange = 20f;

    private void Update()
    {
        if(currentGauge >= data.gaugeMax)
        {
            ActivateSkill();
        }
    }
    protected override void ActivateSkill()
    {
        Collider2D weaponCollider = Physics2D.OverlapCircle(transform.position, skillRange, LayerMask.GetMask("Weapon"));
        if (weaponCollider != null)
        {
            if (WeaponManager.Instance != null)
            {
                WeaponManager.Instance.TakeWeaponLifeDamage();
                Debug.Log("스킬발동 ! ");
            }
            else
            {
                Debug.LogWarning("WeaponManager 인스턴스를 찾을 수 없습니다!");
            }
        }
    }

}
