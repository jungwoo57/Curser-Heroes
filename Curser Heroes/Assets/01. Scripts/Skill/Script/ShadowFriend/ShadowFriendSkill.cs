using System.Linq;
using UnityEngine;

public class ShadowFriendSkill : MonoBehaviour
{
    public GameObject shadowProjectilePrefab;
    
    private Transform cursorTransform;
    private float cooldownTimer;
    private float shootInterval;
    private float nextShootTime;

    private Animator animator;
    private SkillLevelData currentLevelData;
    private SkillManager.SkillInstance skillInstance;

    public void Init(SkillManager.SkillInstance instance)
    {
        this.skillInstance = instance;

        if (WeaponManager.Instance != null && WeaponManager.Instance.cursorWeapon != null)
        {
            this.cursorTransform = WeaponManager.Instance.cursorWeapon.transform;
        }
        else
        {
            Debug.LogWarning("[ShadowFriendSkill] cursorWeapon이 존재하지 않습니다.");
        }

        animator = GetComponent<Animator>();
        currentLevelData = skillInstance.GetCurrentLevelData();
        shootInterval = currentLevelData.cooldown;

        nextShootTime = Time.time + shootInterval;
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            TryFireProjectile();
            ResetCooldown();
        }
    }

    private void ResetCooldown()
    {
        float cooldown = skillInstance.GetCurrentLevelData().cooldown;
        cooldownTimer = Mathf.Max(0.5f, cooldown); // 최소 쿨타임 보장
    }

    private void TryFireProjectile()
    {
        if (cursorTransform == null) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(cursorTransform.position, 30f, LayerMask.GetMask("Monster"));
        if (hits.Length == 0) return;

        // 가장 가까운 적 선택
        Transform target = hits.OrderBy(h => Vector2.Distance(cursorTransform.position, h.transform.position)).First().transform;
        Vector2 direction = (target.position - cursorTransform.position).normalized;

        // 투사체 발사
        GameObject obj = Instantiate(shadowProjectilePrefab, cursorTransform.position, Quaternion.identity);
        obj.GetComponent<ShadowProjectile>().Init(direction, skillInstance);
    }
}