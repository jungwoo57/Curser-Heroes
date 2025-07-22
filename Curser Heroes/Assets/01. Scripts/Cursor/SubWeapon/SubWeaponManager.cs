using UnityEngine;

public class SubWeaponManager : MonoBehaviour
{
    [Header("장착/발사 설정")]
    public SubWeaponData equippedSubWeapon;
    public LayerMask monsterLayer;

    [Header("따라다닐 대상")]
    [Tooltip("보조무기가 회전하며 따라다닐 오브젝트(플레이어/주무기 등)의 Transform")]
    public Transform followTarget;

    [Header("강화 연동")]
    public SubWeaponUpgrade upgradeComponent;

    private int currentAmmo;
    private float currentMana = 100f;
    private float currentChargeTime;
    private float currentCooldown;
    private bool isCharging;
    private bool isReloading;
    private float reloadTimer;

    private bool charged;


    private SubWeaponFollower follower;

    void Awake()
    {
        if (upgradeComponent == null)
            upgradeComponent = GetComponent<SubWeaponUpgrade>()
                               ?? gameObject.AddComponent<SubWeaponUpgrade>();

        follower = GetComponent<SubWeaponFollower>()
                   ?? gameObject.AddComponent<SubWeaponFollower>();
    }

    void Start()
    {
        if (equippedSubWeapon != null)
            EquipSubWeapon(equippedSubWeapon);
    }

    void Update()
    {
        if (equippedSubWeapon == null) return;

        // 쿨다운 감소
        if (currentCooldown > 0f)
            currentCooldown -= Time.deltaTime;

        // 입력 처리 (클릭형 / 충전형)
        if (equippedSubWeapon.weaponType == SubWeaponType.ChargeBased)
        {
            if (Input.GetMouseButton(0))
            {
                isCharging = true;
                currentChargeTime += Time.deltaTime;
            }
            else if (isCharging && CanUseSubWeapon())
            {
                UseSubWeapon();
                isCharging = false;
                currentChargeTime = 0f;
            }
        }
        else if (Input.GetMouseButtonDown(0) && CanUseSubWeapon())
        {
            UseSubWeapon();
        }

        // 마나 회복
        if (equippedSubWeapon.weaponType == SubWeaponType.ManaBased)
            currentMana = Mathf.Min(100f, currentMana + equippedSubWeapon.manaCost * Time.deltaTime);

        // 탄약 리로드
        if (equippedSubWeapon.weaponType == SubWeaponType.AmmoBased)
        {
            if (Input.GetKeyDown(KeyCode.R) || (currentAmmo <= 0 && !isReloading))
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
        if (equippedSubWeapon.weaponType == SubWeaponType.ChargeBased)
        {
            // 1) 마우스 누른 순간 깜빡 시작
            if (Input.GetMouseButtonDown(0))
            {
                isCharging = true;
                currentChargeTime = 0f;
                charged = false;
                follower.StartCharging();
            }

            // 2) 누르고 있는 동안 시간 누적
            if (isCharging && Input.GetMouseButton(0))
            {
                currentChargeTime += Time.deltaTime;
                if (!charged && currentChargeTime >= equippedSubWeapon.requiredChargeTime)
                {
                    charged = true;
                    follower.SetCharged();
                }
            }

            // 3) 마우스에서 손 뗄 때
            if (isCharging && Input.GetMouseButtonUp(0))
            {
                isCharging = false;
                follower.StopCharging();

                if (charged && CanUseSubWeapon())
                {
                    UseSubWeapon();
                }
                // 리셋
                currentChargeTime = 0f;
                charged = false;
            }
        }
        else if (Input.GetMouseButtonDown(0) && CanUseSubWeapon())
        {
            UseSubWeapon();
        }
    }

    public bool CanUseSubWeapon()
    {
        if (currentCooldown > 0f) return false;
        switch (equippedSubWeapon.weaponType)
        {
            case SubWeaponType.AmmoBased: return currentAmmo > 0;
            case SubWeaponType.ManaBased: return currentMana >= equippedSubWeapon.manaCost;
            case SubWeaponType.ChargeBased: return currentChargeTime >= equippedSubWeapon.requiredChargeTime;
            default: return true;
        }
    }

    public void UseSubWeapon()
    {
        currentCooldown = equippedSubWeapon.cooldown;
        if (equippedSubWeapon.weaponType == SubWeaponType.AmmoBased)
            currentAmmo--;
        if (equippedSubWeapon.weaponType == SubWeaponType.ManaBased)
            currentMana -= equippedSubWeapon.manaCost;

        if (equippedSubWeapon.rangeShape == SubWeaponRangeShape.ShortLine)
            UseLineEffectAtCursor();
        else if (equippedSubWeapon.rangeType == SubWeaponRangeType.Radial)
            UseForceEffectAtCursor();
        else
            ShootToNearestEnemy();

        Debug.Log($"발사됨: {equippedSubWeapon.weaponName}, 남은 탄약: {currentAmmo}, 남은 마나: {currentMana}");
    }

    public void EquipSubWeapon(SubWeaponData data)
    {
        equippedSubWeapon = data;
        upgradeComponent.Init(data);

        if (data.weaponType == SubWeaponType.AmmoBased)
            currentAmmo = data.maxAmmo;

        
        follower.Init(data, 0f);
        follower.SetMainWeapon(followTarget, 0f);

        Debug.Log($"보조무기 장착 완료: {data.weaponName}");
    }

    void ShootToNearestEnemy()
    {
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPos.z = 0f;

        var target = FindNearestAliveMonster(cursorPos);
        if (target == null)
        {
            Debug.LogWarning("[Bow] 타겟 몬스터 없음 → 자동조준 실패");
            return;
        }

        Debug.Log($"[Bow] 자동조준 발사: target={target.name}");
        
        GameObject proj = Instantiate(
            equippedSubWeapon.projectilePrefab,
            cursorPos,
            Quaternion.identity
        );
        var sub = proj.GetComponent<SubProjectile>();
        if (sub != null)
            sub.Init(
                equippedSubWeapon,
                target,
                Mathf.RoundToInt(upgradeComponent.GetCurrentDamage())
            );
    }

    void StartReloading()
    {
        isReloading = true;
        reloadTimer = equippedSubWeapon.reloadTime;
        Debug.Log("리로드 시작");
    }

    BaseMonster FindNearestAliveMonster(Vector3 from)
    {
        var monsters = FindObjectsOfType<BaseMonster>();
        Debug.Log($"[AutoAim] Monsters total: {monsters.Length}");
        BaseMonster nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var m in monsters)
           
        {
            Debug.Log($"[AutoAim]  → {m.name}, IsDead={m.IsDead}, CurrentHP={m.CurrentHP}");
            if (m.IsDead) continue;
            float dist = Vector2.Distance(from, m.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = m;
            }
        }

        Debug.Log($"[AutoAim] Found {monsters.Length} monsters, nearest={(nearest != null ? nearest.name : "null")}");
        return nearest;
    }

    void UseForceEffectAtCursor()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0f;

        var proj = Instantiate(
            equippedSubWeapon.projectilePrefab, pos, Quaternion.identity);
        var rp = proj.GetComponent<RadialProjectile>();
        if (rp == null)
        {
            Debug.LogError("RadialProjectile 없음");
            Destroy(proj);
            return;
        }

        // 3) 데미지만 매니저에서 덮어쓰기
        rp.damage = Mathf.RoundToInt(upgradeComponent.GetCurrentDamage());
        rp.monsterLayer = monsterLayer;

    }


    void UseLineEffectAtCursor()
    {
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPos.z = 0f;

      
        BaseMonster target = FindNearestAliveMonster(cursorPos);

       
        Vector3 aimPos = cursorPos;
        if (target != null)
        {
            var sr = target.GetComponent<SpriteRenderer>();
            aimPos = (sr != null) ? sr.bounds.center : target.transform.position;
        }

      
        Vector3 dir = (aimPos - cursorPos).normalized;
        float angleDeg = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;


        float cursorRadius = 0.5f;
        Vector3 origin = cursorPos + dir * cursorRadius;


        GameObject proj = Instantiate(
            equippedSubWeapon.projectilePrefab,
            origin,
            Quaternion.Euler(0, 0, angleDeg)
        );


        var lp = proj.GetComponent<LineProjectile>();
        if (lp == null)
        {
            Debug.LogError("LineProjectile 컴포넌트가 없습니다!");
            Destroy(proj);
            return;
        }
       
        float maxLen = equippedSubWeapon.projectileMaxDistance;
        float distToAim = Vector2.Distance(cursorPos, aimPos);
        lp.length = Mathf.Min(maxLen, distToAim);
        lp.width = equippedSubWeapon.effectWidth;
        lp.damage = Mathf.RoundToInt(upgradeComponent.GetCurrentDamage());
        lp.monsterLayer = monsterLayer;
        lp.applyStun = equippedSubWeapon.stunOnLine;
        lp.stunDuration = equippedSubWeapon.stunDuration;
    }





}
