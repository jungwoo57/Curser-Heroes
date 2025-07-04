using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CursorWeapon : MonoBehaviour
{
    public WeaponData currentWeapon;    //무기 정보
    public LayerMask targetLayer;       //공격대상 설정
    public WeaponLife weaponLife;       // 분리된 목숨 관리 
    public WeaponUpgrade weaponUpgrade;      // 무기 레벨 관리

    private Dictionary<BaseMonster, float> lastHitTimesBase = new Dictionary<BaseMonster, float>();
   

    //공격 쿨타임을 위해 몬스터 별로 마지막 공격한 시간을 저장, 몬스터 마다 각각 쿨타임을 적용할 수 있다.

    private Camera cam;      // 마우스 좌표를 월드 좌표로 바꾸기 위해 메인 카메라를 참조.

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {      
        AutoAttackCursor();      //커서 근처에 있는 몬스터를 감지하고 쿨타임에 따라 자동으로 공격, 프레임마다 호출
    } 

    private void AutoAttackCursor()      //커서의 좌표설정 
    {
        Vector3 mousePos = Input.mousePosition;             
        Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);
        
        Vector2 cursorPos = new Vector2(worldPos.x, worldPos.y);

        if (currentWeapon == null || weaponUpgrade == null) return;

        float range = currentWeapon.attackRange;         //커서의 범위 값
        float cooldown = currentWeapon.attackCooldown;   //쿨타임 값
        float damage = currentWeapon.GetDamage(weaponUpgrade.weaponLevel); // 강화레벨을 포함 시킨 무기 공격력 값

        Collider2D[] hits = Physics2D.OverlapCircleAll(cursorPos, range, targetLayer);   // 커서 위치를 중심으로 원으로 범위 탐지

        foreach (var hit in hits)
        {
            // 일반 몬스터 감지
            BaseMonster monster = hit.GetComponent<BaseMonster>();
            if (monster != null)
            {
                if (!lastHitTimesBase.TryGetValue(monster, out float lastHitTime))
                    lastHitTime = 0f;

                if (Time.time - lastHitTime >= cooldown)
                {
                    monster.TakeDamage(Mathf.RoundToInt(damage));
                    AudioManager.Instance.PlayHitSound(HitType.Cursor);
                    lastHitTimesBase[monster] = Time.time;

                    TryTriggerMeteorSkill();
                }
                continue;
            }

            // 보스 몬스터 감지
            //BossBaseMonster boss = hit.GetComponent<BossBaseMonster>();
            //if (boss != null)
            //{
            //    if (!lastHitTimesBoss.TryGetValue(boss, out float lastHitTime))
            //        lastHitTime = 0f;

            //    if (Time.time - lastHitTime >= cooldown)
            //    {
            //        boss.TakeDamage(Mathf.RoundToInt(damage));
            //        AudioManager.Instance.PlayHitSound(HitType.Cursor);
            //        lastHitTimesBoss[boss] = Time.time;

            //        TryTriggerMeteorSkill();
            //    }
            //}
        }


    }
    private void TryTriggerMeteorSkill()
    {
        var skillManager = FindObjectOfType<SkillManager>();
        if (skillManager == null) return;

        var meteorSkill = skillManager.ownedSkills.FirstOrDefault(s => s.skill.skillName == "별동별");
        if (meteorSkill != null)
        {
            skillManager.TrySpawnMeteorSkill(meteorSkill);
        }
    }
    public void SetWeapon(WeaponData weaponData)     //외부에서 무기를 장착할 수 있게 해주는 초기화 함수
    {
        currentWeapon = weaponData;
    }

    private void OnDrawGizmos()       //레인지 범위 시각효과(에디터 전용) 
    {
        if (currentWeapon == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, currentWeapon.attackRange);
    }



}
