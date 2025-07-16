using UnityEngine;
using System.Collections;

public class RabbitGirl : BasePartner
{
    [Header("스킬 범위")]
    public float skillRange = 20f;

    protected override void ActivateSkill()
    {
        //7초동안 무적 및 공격력 증가
        StartCoroutine(WeaponManager.Instance.OnTemporaryInvincible(7f));
        StartCoroutine(ApplyDoubleDamageForSeconds(7f));
        //스킬 사용 후 게이지 초기화
        currentGauge = 0f;
        ui.UpdateGauge(0f);
    }
    private IEnumerator ApplyDoubleDamageForSeconds(float duration)
    {
        CursorWeapon weapon = WeaponManager.Instance.cursorWeapon;
        if (weapon == null) yield break;

        weapon.damageMultiplier = 2f;
        Debug.Log("[RabbitGirl] 7초 동안 공격력 2배 버프 적용");
        yield return new WaitForSeconds(duration);
        weapon.damageMultiplier = 1f;
        Debug.Log("[RabbitGirl] 공격력 버프 해제");
    }
}
