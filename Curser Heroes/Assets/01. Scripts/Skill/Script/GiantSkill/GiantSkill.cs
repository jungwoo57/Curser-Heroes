using UnityEngine;

public class GiantSkill : MonoBehaviour
{
    private bool alreadyActivated = false;

    public void Activate(SkillManager.SkillInstance skillInstance)
    {
        if (alreadyActivated) return;
        alreadyActivated = true;

        AudioSource audioSource = GetComponent<AudioSource>();

        if (audioSource != null && skillInstance.skill.audioClip != null)
        {
            audioSource.PlayOneShot(skillInstance.skill.audioClip);
        }

        // 1) 최대 목숨 +1, 회복도 +1
        WeaponLife weaponLife = WeaponManager.Instance.cursorWeapon.GetComponent<WeaponLife>();
        if (weaponLife != null)
        {
            weaponLife.IncreaseMaxLife(1);
        }

        // 2) 공격 범위 배율 1.5배 증가 + 커서 크기 1.5배 증가
        CursorWeapon cursorWeapon = WeaponManager.Instance.cursorWeapon;
        if (cursorWeapon != null)
        {
            cursorWeapon.attackRangeMultiplier *= 1.5f;
            cursorWeapon.transform.localScale *= 1.5f;

            // 3) 공격 범위 콜라이더 크기 확대 (예: CircleCollider2D)
            CircleCollider2D circleCollider = cursorWeapon.GetComponent<CircleCollider2D>();
            if (circleCollider != null)
            {
                circleCollider.radius *= 1.5f;
            }
        }
    }
}