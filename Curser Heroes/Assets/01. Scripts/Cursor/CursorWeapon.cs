using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CursorWeapon : MonoBehaviour
{
    public WeaponData currentWeapon;    // 무기 정보
    public LayerMask targetLayer;       // 공격 대상 설정
    public WeaponLife weaponLife;       // 분리된 목숨 관리
    public WeaponUpgrade weaponUpgrade; // 무기 레벨 관리
    public SpriteRenderer weaponSprite;
    private int sweepAttackCounter = 0;
    private Dictionary<BaseMonster, float> lastHitTimesBase = new Dictionary<BaseMonster, float>();
    private Dictionary<BossStats, float> lastHitTimesBoss = new Dictionary<BossStats, float>();
    private Dictionary<BossBase, float> lastHitTimesBossB = new Dictionary<BossBase, float>();
    private BaseMonster lastHitMonster;

    public static event Action<CursorWeapon> OnAnyMonsterDamaged;

    private Collider2D collider;

    private Camera cam; // 마우스 좌표를 월드 좌표로 바꾸기 위해 메인 카메라 참조
    public float damageMultiplier = 1f;
    public float attackRangeMultiplier = 1f;

    void Start()
    {
        cam = Camera.main;
        collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (WeaponManager.Instance.isDie) return;
        // AutoAttackCursor(); // 커서 근처 몬스터를 감지하고 쿨타임에 따라 공격
    }

    private void AutoAttackCursor()
    {
        //Vector3 mousePos = Input.mousePosition;
        //Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);

        //Vector2 cursorPos = new Vector2(worldPos.x, worldPos.y);

        if (currentWeapon == null || weaponUpgrade == null) return;

        float range = currentWeapon.attackRange * attackRangeMultiplier;
        float cooldown = currentWeapon.attackCooldown;
        float damage = GetCurrentDamage(); //  변경된 부분: 데미지 계산 함수 호출

        //Collider2D[] hits = Physics2D.OverlapCircleAll(cursorPos, range, targetLayer);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range, targetLayer);
        foreach (var hit in hits)
        {
            BaseMonster monster = hit.GetComponent<BaseMonster>();
            if (monster != null)
            {
                if (!lastHitTimesBase.TryGetValue(monster, out float lastHitTime))
                    lastHitTime = 0f;

                if (Time.time - lastHitTime >= cooldown)
                {
                    sweepAttackCounter++;

                    int finalDamage = Mathf.RoundToInt(damage);
                    int triggerCount = SkillManager.Instance.criticalSweepEveryNth;

                    if (triggerCount > 0)
                    {
                        Debug.Log($"[약점 포착] {triggerCount}회 중 {sweepAttackCounter}회 공격 진행 중");

                        if (sweepAttackCounter >= triggerCount)
                        {
                            finalDamage *= 2;
                            Debug.Log($"[약점 포착] {triggerCount}회 달성 → 2배 피해!");
                            sweepAttackCounter = 0;
                        }
                    }

                    monster.TakeDamage(finalDamage);
                    AudioManager.Instance.PlayHitSound(HitType.Cursor);
                    lastHitTimesBase[monster] = Time.time;
                    lastHitMonster = monster;

                    Debug.Log($"[커서공격] 일반몬스터에게 {finalDamage} 데미지 입힘");

                    TryTriggerMeteorSkill();
                    TryTriggerLightningSkill();
                    //SkillManager.Instance.TryTriggerDimensionSlash(cursorPos);
                    SkillManager.Instance.TryTriggerDimensionSlash(transform.position);
                    OnAnyMonsterDamaged?.Invoke(this);
                }

                continue;
            }

            BossStats boss = hit.GetComponent<BossStats>();
            if (boss != null)
            {
                if (!lastHitTimesBoss.TryGetValue(boss, out float lastHitTime))
                    lastHitTime = 0f;

                if (Time.time - lastHitTime >= cooldown)
                {
                    sweepAttackCounter++;

                    int finalDamage = Mathf.RoundToInt(damage);
                    int triggerCount = SkillManager.Instance.criticalSweepEveryNth;

                    if (triggerCount > 0)
                    {
                        Debug.Log($"[약점 포착] {triggerCount}회 중 {sweepAttackCounter}회 공격 진행 중");

                        if (sweepAttackCounter >= triggerCount)
                        {
                            finalDamage *= 2;
                            Debug.Log($"[약점 포착] {triggerCount}회 달성 → 2배 피해!");
                            sweepAttackCounter = 0;
                        }
                    }

                    boss.TakeDamage(finalDamage);
                    AudioManager.Instance.PlayHitSound(HitType.Cursor);
                    lastHitTimesBoss[boss] = Time.time;
                    lastHitMonster = monster;

                    TryTriggerMeteorSkill();
                    TryTriggerLightningSkill();
                    //SkillManager.Instance.TryTriggerDimensionSlash(cursorPos);
                    SkillManager.Instance.TryTriggerDimensionSlash(transform.position);
                    OnAnyMonsterDamaged?.Invoke(this);
                }
            }
            BossBase bossB = hit.GetComponent<BossBase>();
            if (bossB != null)
            {
                if (!lastHitTimesBossB.TryGetValue(bossB, out float lastHitTime))
                    lastHitTime = 0f;

                if (Time.time - lastHitTime >= cooldown)
                {
                    sweepAttackCounter++;

                    int finalDamage = Mathf.RoundToInt(damage);
                    int triggerCount = SkillManager.Instance.criticalSweepEveryNth;

                    if (triggerCount > 0)
                    {
                        Debug.Log($"[약점 포착] {triggerCount}회 중 {sweepAttackCounter}회 공격 진행 중");

                        if (sweepAttackCounter >= triggerCount)
                        {
                            finalDamage *= 2;
                            Debug.Log($"[약점 포착] {triggerCount}회 달성 → 2배 피해!");
                            sweepAttackCounter = 0;
                        }
                    }

                    bossB.TakeDamage(finalDamage);
                    AudioManager.Instance.PlayHitSound(HitType.Cursor);
                    lastHitTimesBossB[bossB] = Time.time;
                    lastHitMonster = monster;

                    TryTriggerMeteorSkill();
                    TryTriggerLightningSkill();
                    //SkillManager.Instance.TryTriggerDimensionSlash(cursorPos);
                    SkillManager.Instance.TryTriggerDimensionSlash(transform.position);
                    OnAnyMonsterDamaged?.Invoke(this);
                }
            }

            
        }
        
        
    }

    public int GetCurrentDamage()
    {
        if (currentWeapon == null || weaponUpgrade == null) return 0;

        float damage = currentWeapon.GetDamage(weaponUpgrade.weaponLevel);

        var strengthSkill = SkillManager.Instance.ownedSkills
            .Find(s => s.skill.skillName == "근력 훈련");

        if (strengthSkill != null)
        {
            int bonusDamage = strengthSkill.skill.levelDataList[strengthSkill.level - 1].damage;
            damage += bonusDamage;
        }

        damage *= damageMultiplier;

        return Mathf.RoundToInt(damage);
    }

    private void TryTriggerMeteorSkill()
    {
        if (SkillManager.Instance == null) return;

        var meteorSkill = SkillManager.Instance.ownedSkills.FirstOrDefault(s => s.skill.skillName == "별동별");
        if (meteorSkill != null)
        {
            SkillManager.Instance.TrySpawnMeteorSkill(meteorSkill);
        }
    }

    private void TryTriggerLightningSkill()
    {
        if (SkillManager.Instance.lightningSkill == null || lastHitMonster == null)
            return;

        SkillManager.Instance.lightningSkill.TryTriggerLightning(lastHitMonster);
    }

    public void SetWeapon(WeaponData weaponData)
    {
        currentWeapon = weaponData;
        weaponSprite.sprite = currentWeapon.weaponImage;
    }

    private void OnDrawGizmos()
    {
        if (currentWeapon == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, currentWeapon.attackRange);
    }

    public void ResetSweepCounter()
    {
        sweepAttackCounter = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            Debug.Log("닿긴함");
            AutoAttackCursor();
        }
    }
}