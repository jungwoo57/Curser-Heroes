using UnityEngine;
using System.Collections;

public class RabbitGirl : BasePartner
{
    public GameObject carrotEffectPrefab;
    public GameObject rabbitEffect;
    [SerializeField] private float imageEnableTime;
    [Header("스킬 범위")]
    public float skillRange = 20f;

    private bool isSkillActive = false;
    protected override void ActivateSkill()
    {
        if (isSkillActive) return;  // 이미 활성화 중이면 무시

        isSkillActive = true;
        StartCoroutine(SkillRoutine());
        rabbitEffect.SetActive(true);
        StartCoroutine(RabbitImageDisAble());
    }

    private IEnumerator SkillRoutine()
    {
        // 기존 스킬 로직
        StartCoroutine(WeaponManager.Instance.OnTemporaryInvincible(7f));
        StartCoroutine(ApplyDoubleDamageForSeconds(7f));
        PlayCarrotThrowEffect();

        // 7초 대기 후 스킬 비활성화
        yield return new WaitForSeconds(7f);
        isSkillActive = false;

        currentGauge = 0f;
        ui.UpdateGauge(0f);
    }

    private void PlayCarrotThrowEffect()
    {
        if (carrotEffectPrefab == null) return;

        Vector3 center = Vector3.zero;
        Vector3 cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursor.z = 0f;

        // 시작 위치: 동료 기준 외곽 방향
        Vector3 spawnPos = GetOffScreenPositionFromDirection(transform.position);

        GameObject obj = Instantiate(carrotEffectPrefab, spawnPos, Quaternion.identity);

        RabbitGirlEffect effect = obj.GetComponent<RabbitGirlEffect>();
        if (effect != null)
        {
            effect.StartThrow(center, cursor);
        }
    }

    // 동료에서 화면 중심 방향의 외곽 위치 계산
    private Vector3 GetOffScreenPositionFromDirection(Vector3 from)
    {
        Vector3 dir = (Vector3.zero - from).normalized;
        return from + dir * 20f;
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

    IEnumerator RabbitImageDisAble()
    {
        yield return new WaitForSeconds(imageEnableTime);
        rabbitEffect.SetActive(false);
    }
}