using UnityEngine;

public class IndomitableSkill : MonoBehaviour
{
    private SkillManager.SkillInstance skillInstance;
    private Transform player;
    private bool isOnCooldown;

    [Header("이펙트 프리팹")]
    [SerializeField] private GameObject IndomitableEffect;   // 평소에 항상 보이는 방패 이미지
    [SerializeField] private GameObject IndomitableHitPrefab;    // 피격 시 잠깐 보여줄 이펙트
    [SerializeField] private float shieldEffectDuration = 0.5f;

    private GameObject shieldIdleInstance;

    // 초기화
    public void Init(SkillManager.SkillInstance skillInstance, Transform playerTransform)
    {
        this.skillInstance = skillInstance;
        this.player = playerTransform;
        isOnCooldown = false;

        if (IndomitableEffect != null)
        {
            // 부모로 붙이지 않고 독립 오브젝트로 생성
            shieldIdleInstance = Instantiate(IndomitableEffect, player.position, Quaternion.identity);
            // 부모 설정 안 함 (RotatingShieldSkill 스타일과 통일)
        }
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("[IndomitableSkill] player가 null입니다.");
            return;
        }

        // IndomitableSkill 오브젝트 자체도 플레이어 위치 따라다니도록
        transform.position = player.position;

        // 보호막 이펙트 위치도 플레이어 위치에 맞게 업데이트
        if (shieldIdleInstance != null)
        {
            shieldIdleInstance.transform.position = player.position;
        }
    }

    // 피해 방어 시도
    public bool TryBlockDamage()
    {
        if (isOnCooldown) return false;

        Debug.Log("[불굴] 피해 1회 방어!");

        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && skillInstance.skill.audioClip != null)
        {
            audioSource.PlayOneShot(skillInstance.skill.audioClip);
        }

        DeployShieldEffect();

        if (shieldIdleInstance != null)
            shieldIdleInstance.SetActive(false);

        isOnCooldown = true;
        float cooldown = skillInstance.skill.levelDataList[skillInstance.level - 1].cooldown;
        Invoke(nameof(ResetCooldown), cooldown);

        return true;
    }

    // 쿨다운 끝나면 보호막 다시 표시
    private void ResetCooldown()
    {
        isOnCooldown = false;

        if (shieldIdleInstance != null)
            shieldIdleInstance.SetActive(true);
    }

    // 피격 시 이펙트 생성
    private void DeployShieldEffect()
    {
        if (IndomitableHitPrefab == null) return;

        GameObject effect = Instantiate(IndomitableHitPrefab, player.position, Quaternion.identity);
        Destroy(effect, shieldEffectDuration);
    }

    private void OnDestroy()
    {
        if (shieldIdleInstance != null)
            Destroy(shieldIdleInstance);
    }
}