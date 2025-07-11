using System;
using UnityEngine;


public class SubWeaponManager : MonoBehaviour
{
    public SubWeaponData equippedSubWeapon;    //현재 장착중인 보조무기 데이터
<<<<<<< HEAD
  
=======
    [SerializeField]private float currentCooldown = 0f;      //현재 쿨타임 남은시간
>>>>>>> Develop
    public LayerMask monsterLayer;


    private float currentCooldown = 0f;        // 쿨타임
    private int currentAmmo;
    private float currentMana = 100f;
    private float currentChargeTime = 0f;
    private bool isCharging = false;

    private bool isReloading = false;
    private float reloadTimer = 0f;

    public float manaRegenPerSecond = 5f;




    void Start()       //장탄형 무기 탄약 초기화 >> 무기 장착시 탄약을 최대치로 초기화
    {
        if (equippedSubWeapon.weaponType == SubWeaponType.AmmoBased)
            currentAmmo = equippedSubWeapon.maxAmmo;
    }



    void Update()
    {
        //쿨타임 감소
        if (currentCooldown > 0f)  
            currentCooldown -= Time.deltaTime;

        // 충전형 무기
        if (equippedSubWeapon.weaponType == SubWeaponType.ChargeBased)
        {
<<<<<<< HEAD
            if (Input.GetMouseButton(0))
            {
                isCharging = true;
                currentChargeTime += Time.deltaTime;
            }
            else
            {
                if (isCharging && CanUseSubWeapon())
                    UseSubWeapon();

                isCharging = false;
                currentChargeTime = 0f;
            }
        }
        else
        {
            // 클릭형 무기
            if (Input.GetMouseButtonDown(0) && CanUseSubWeapon())
            {
                UseSubWeapon();
            }
        }

        // 마나 자동 회복
        if (equippedSubWeapon.weaponType == SubWeaponType.ManaBased)
        {
            currentMana += manaRegenPerSecond * Time.deltaTime;
            currentMana = Mathf.Min(currentMana, 100f);
        }

        // 탄약 무기 리로드
        if (equippedSubWeapon.weaponType == SubWeaponType.AmmoBased)
        {
            if (Input.GetKeyDown(KeyCode.R))
                StartReloading();

            if (currentAmmo <= 0 && !isReloading)
                StartReloading();

            if (isReloading)
            {
                reloadTimer -= Time.deltaTime;
                if (reloadTimer <= 0f)
                {
                    currentAmmo = equippedSubWeapon.maxAmmo;
                    isReloading = false;
                    Debug.Log(" 리로드 완료!");
                }
            }
        }

=======
            UseSubWeapon();

            SkillManager.Instance.TryShootFireball(); // 클릭 공격 시 화염구 스킬 발동을 위해 추가
        }              //마우스 좌클릭시 보조무기를 사용할 수 있는지 체크하고 사용
>>>>>>> Develop
    }



    public bool CanUseSubWeapon()
    {
        if (equippedSubWeapon == null || currentCooldown > 0f)
            return false;

        switch (equippedSubWeapon.weaponType)
        {
            case SubWeaponType.AmmoBased:
                return currentAmmo > 0;

            case SubWeaponType.ManaBased:
                return currentMana >= equippedSubWeapon.manaCost;

            case SubWeaponType.ChargeBased:
                return currentChargeTime >= equippedSubWeapon.requiredChargeTime;

            default:
                return true;
        }
    }

    public void UseSubWeapon()
    {
        currentCooldown = equippedSubWeapon.cooldown;

        switch (equippedSubWeapon.weaponType)
        {
            case SubWeaponType.AmmoBased:
                currentAmmo--;
                break;
            case SubWeaponType.ManaBased:
                currentMana -= equippedSubWeapon.manaCost;
                break;
        }

        if (equippedSubWeapon.rangeType == SubWeaponRangeType.Radial)
            ShootAreaAroundCursor();  //포스 이펙트
        else
            ShootToNearestEnemy();   //자동 조준

        Debug.Log($"발사됨: {equippedSubWeapon.weaponName}");
        Debug.Log($"현재 탄약: {currentAmmo}");
        Debug.Log($"현재 마나: {currentMana}");
        Debug.Log($"차징 시간: {currentChargeTime}");

    }

    void ShootToNearestEnemy()   //자동조준 발사
    {
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);   //메인카메라에서 커서의 좌표
        cursorPos.z = 0f;

        BaseMonster target = FindNearestAliveMonster(cursorPos);    //커서 주변에서 가장 가까운 몬스터 탐색
        if (target == null)
        {
            Debug.LogWarning("타겟 몬스터 없음");
            return;
        }

       

        GameObject proj = Instantiate(equippedSubWeapon.projectilePrefab, cursorPos, Quaternion.identity);   //투사체 프리팹을 커서 위치에 생성
        SubProjectile sub = proj.GetComponent<SubProjectile>();    //생성된 투사체 프리팹 가져오기
        if (sub != null)
        {
            sub.Init(equippedSubWeapon, target);   //타켓 좌표와 장착된 보조무기 정보를 넘기고 초기화
        }
        else
        {
            Debug.LogError("SubProjectile 컴포넌트가 프리팹에 없습니다!");
        }
    }


    void ShootAreaAroundCursor()
    {
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPos.z = 0f;

        int damage = Mathf.RoundToInt(equippedSubWeapon.GetDamage());
        float radius = equippedSubWeapon.projectileMaxDistance;

        // Layer 설정: "Monster" 레이어에만 영향
        LayerMask monsterLayer = LayerMask.GetMask("Monster");

        // ForceEffect 인스턴스 생성 (이름 없이 순서대로)
        IEffect forceEffect = new ForceEffect(cursorPos, damage, radius, monsterLayer);

        // EffectManager를 담은 임시 오브젝트 생성
        GameObject effectObj = new GameObject("ForceEffectHost");
        effectObj.transform.position = cursorPos;

        EffectManager effectManager = effectObj.AddComponent<EffectManager>();
        effectManager.Init(null); // 포스 이펙트는 특정 몬스터와 연결 없음
        effectManager.AddEffect(forceEffect);

        // 이펙트 종료 후 오브젝트 삭제
        Destroy(effectObj, 0.5f);
    }



    BaseMonster FindNearestAliveMonster(Vector3 from)     //가장 가까운 몬스터 탐색
    {
        BaseMonster[] monsters = FindObjectsOfType<BaseMonster>();    
        BaseMonster nearest = null;      
        float minDist = Mathf.Infinity;         //가장 가까운 거리(처음엔 무한대)

        foreach (var m in monsters)
        {
            if (m.IsDead) continue;   //이미 죽은 상태면 패스

            float dist = Vector2.Distance(from, m.transform.position);  //커서의 위치와 몬스터의 현재 거리를 계산
            if (dist < minDist)
            {
                minDist = dist;  //최소 거리 갱신
                nearest = m;     //가장 가까운 몬스터 저장
            }
        }

        return nearest;
    }
    void StartReloading()
    {
        isReloading = true;
        reloadTimer = equippedSubWeapon.reloadTime;
        Debug.Log("🔃 리로드 중...");
    }


}
