using System;
using UnityEngine;

public class SubWeaponManager : MonoBehaviour
{
    public SubWeaponData equippedSubWeapon;    //현재 장착중인 보조무기 데이터

    [SerializeField] private float currentCooldown = 0f;      //현재 쿨타임 남은시간
    public LayerMask monsterLayer;

    public int currentAmmo;

    private float currentMana = 100f;
    private float currentChargeTime = 0f;
    private bool isCharging = false;

    private bool isReloading = false;
    private float reloadTimer = 0f;

    public float manaRegenPerSecond = 5f;

    public GameObject subWeaponVisualPrefab;  // 보조무기 외형 프리팹
    private GameObject currentVisual;

    void Start() //장탄형 무기 탄약 초기화 >> 무기 장착시 탄약을 최대치로 초기화
    {
        if (equippedSubWeapon != null)
        {
            EquipSubWeapon(equippedSubWeapon);  //  장착 함수로 초기화 처리
        }
    }

    void Update()
    {
        if (equippedSubWeapon == null)
            return; // 무기 미장착 시 로직 중단

        //쿨타임 감소
        if (currentCooldown > 0f)
            currentCooldown -= Time.deltaTime;

        // 충전형 무기
        if (equippedSubWeapon.weaponType == SubWeaponType.ChargeBased)
        {
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
                    Debug.Log("리로드 완료!");
                }
            }
        }
    }

    public bool CanUseSubWeapon()
    {
        if (equippedSubWeapon == null || currentCooldown > 0f)
        {
            Debug.Log(" 무기를 사용할 수 없음: 쿨타임이 남았거나 무기가 없음");
            return false;
        }

        switch (equippedSubWeapon.weaponType)
        {
            case SubWeaponType.AmmoBased:
                if (currentAmmo > 0)
                {
                    Debug.Log($" 장탄형 무기 사용 가능 - 남은 탄약: {currentAmmo}");
                    return true;
                }
                else
                {
                    Debug.Log(" 탄약 없음. 장전 필요!");
                    return false;
                }

            case SubWeaponType.ManaBased:
                Debug.Log($" 마나 보유량: {currentMana}");
                return currentMana >= equippedSubWeapon.manaCost;

            case SubWeaponType.ChargeBased:
                Debug.Log($" 현재 차징 시간: {currentChargeTime}");
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
            sub.Init(equippedSubWeapon, target);   //타겟 좌표와 장착된 보조무기 정보를 넘기고 초기화
        }
        else
        {
            Debug.LogError("SubProjectile 컴포넌트가 프리팹에 없습니다!");
        }

        SubWeaponFollower follower = proj.GetComponent<SubWeaponFollower>();
        if (follower != null)
        {
            // 메인 무기를 기준으로 하지 않음
            follower.SetTarget(target.transform);
        }
    }

    void ShootAreaAroundCursor()
    {
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPos.z = 0f;

        int damage = Mathf.RoundToInt(equippedSubWeapon.GetDamage());
        float radius = equippedSubWeapon.projectileMaxDistance;

        LayerMask monsterLayer = LayerMask.GetMask("Monster");

        IEffect forceEffect = new ForceEffect(cursorPos, damage, radius, monsterLayer);

        GameObject effectObj = new GameObject("ForceEffectHost");
        effectObj.transform.position = cursorPos;

        EffectManager effectManager = effectObj.AddComponent<EffectManager>();
        effectManager.Init(null); // 포스 이펙트는 특정 몬스터와 연결 없음
        effectManager.AddEffect(forceEffect);

        Destroy(effectObj, 0.5f);
    }

    BaseMonster FindNearestAliveMonster(Vector3 from)     //가장 가까운 몬스터 탐색
    {
        BaseMonster[] monsters = FindObjectsOfType<BaseMonster>();
        BaseMonster nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var m in monsters)
        {
            if (m.IsDead) continue;

            float dist = Vector2.Distance(from, m.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = m;
            }
        }

        return nearest;
    }

    void StartReloading()
    {
        Debug.Log("StartReloading 호출됨");
        isReloading = true;
        reloadTimer = equippedSubWeapon.reloadTime;
        Debug.Log(" 리로드 중..");
    }

    public void EquipSubWeapon(SubWeaponData newWeaponData)
    {
        equippedSubWeapon = newWeaponData;

        if (equippedSubWeapon.weaponType == SubWeaponType.AmmoBased)
            currentAmmo = equippedSubWeapon.maxAmmo;

        if (currentVisual != null)
            Destroy(currentVisual);

        if (subWeaponVisualPrefab != null)
        {
            currentVisual = Instantiate(subWeaponVisualPrefab, transform.position, Quaternion.identity);
            SubWeaponFollower followerVisual = currentVisual.GetComponent<SubWeaponFollower>();
            if (followerVisual != null)
            {
                // 메인 무기 기준 이동 제거됨
                followerVisual.Init(equippedSubWeapon);
            }
        }

        Debug.Log($" 보조무기 장착 완료: {equippedSubWeapon.weaponName}");
    }
}
