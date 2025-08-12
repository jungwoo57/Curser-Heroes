using UnityEngine;

public class SalvationSkill : MonoBehaviour
{
    private bool hasActivated = false;
    private SkillManager.SkillInstance skillInstance;
    private Transform player;

    [SerializeField] private Sprite usedIcon; // 발동 후 아이콘

    public void Init(SkillManager.SkillInstance skillInstance, Transform player)
    {
        this.skillInstance = skillInstance;
        this.player = player;
    }

    public bool TryActivate()
    {
        if (hasActivated) return false;

        hasActivated = true;

        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && skillInstance.skill.audioClip != null)
        {
            audioSource.PlayOneShot(skillInstance.skill.audioClip);
        }

        Debug.Log("[구원] 발동! 목숨 1 회복 + 무적 5초");

        // 목숨 1 회복
        WeaponManager.Instance.weaponLife.RecoverLife();

        // 무적 상태 5초
        WeaponManager.Instance.StartCoroutine(WeaponManager.Instance.OnTemporaryInvincible(5f));

        // 아이콘 교체
        UIManager.Instance.battleUI.UpdateSkillIcon(skillInstance.skill.skillName, usedIcon);

        return true;
    }
}