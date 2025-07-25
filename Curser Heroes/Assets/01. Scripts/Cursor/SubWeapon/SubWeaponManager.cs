using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SubWeaponManager : MonoBehaviour
{
    [Header("장착/발사 설정")]
    public SubWeaponData equippedSubWeapon;
    public LayerMask monsterLayer;

    [Header("따라다닐 대상")]
    public Transform followTarget;

    [Header("강화 연동")]
    public SubWeaponUpgrade upgradeComponent;

    [Header("UI 세팅")]
    public Canvas uiCanvas;
    public RectTransform uiPanel;

    [Header("UI 그룹")]
    public GameObject AmmoUIGroup;
    public GameObject ManaUIGroup;
    public GameObject ChargeUIGroup;

    // 장탄형 UI
    public Image ammoIconPrefab;
    public Sprite usedAmmoSprite;
    private List<Image> ammoIcons = new List<Image>();

    // 마나형 UI
    public Image manaBar;
    private float maxMana = 100f;
    private float currentMana;

    // 차징형 UI
    public Image chargeEmptyBar;
    public Image chargeCompleteBar;
    private float currentChargeTime;
    private bool isCharging;
    private bool charged;

    // 공통 상태
    private int maxAmmo;
    private int currentAmmo;
    private float currentCooldown;


    private SubWeaponFollower follower;

    private Coroutine manaFireCoroutine;

    [Header("Mana Regeneration")]
    public float manaRegenRate = 10f; // 초당 회복량


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
        if (GameManager.Instance)
        {
            equippedSubWeapon = GameManager.Instance.subEquipWeapon.data; // 게임매니저가 있으면 넣어주기
        }

        uiPanel.gameObject.SetActive(false);
        HideAllUI();

        if (equippedSubWeapon != null)
            EquipSubWeapon(equippedSubWeapon);
    }

    void Update()
    {
        if (equippedSubWeapon == null) return;

        //  UI 패널 위치 고정
        Vector3 screenPos = Camera.main.WorldToScreenPoint(followTarget.position);
        uiPanel.position = screenPos + Vector3.down * 30f;

        //  쿨다운 감소 
        if (currentCooldown > 0f)
            currentCooldown -= Time.deltaTime;

        //  UI 그룹 토글 & 갱신
        AmmoUIGroup.SetActive(equippedSubWeapon.weaponType == SubWeaponType.AmmoBased);
        ManaUIGroup.SetActive(equippedSubWeapon.weaponType == SubWeaponType.ManaBased);
        ChargeUIGroup.SetActive(equippedSubWeapon.weaponType == SubWeaponType.ChargeBased);

        // 마나 자동 회복 (언제나)
        if (equippedSubWeapon.weaponType == SubWeaponType.ManaBased)
            currentMana = Mathf.Min(maxMana, currentMana + manaRegenRate * Time.deltaTime);

        switch (equippedSubWeapon.weaponType)
        {
            case SubWeaponType.AmmoBased:
                UpdateAmmoUI();
                break;
            case SubWeaponType.ManaBased:
                UpdateManaUI();
                break;
            case SubWeaponType.ChargeBased:
                UpdateChargeUI();
                break;
        }

        
        switch (equippedSubWeapon.weaponType)
        {
            case SubWeaponType.AmmoBased:
                if (Input.GetMouseButtonDown(0) && CanUseSubWeapon())
                    UseSubWeapon();
                break;

            case SubWeaponType.ManaBased:
                if (Input.GetMouseButtonDown(0))
                {
                    
                    if (manaFireCoroutine == null)
                        manaFireCoroutine = StartCoroutine(ContinuousManaFire());
                }
                if (Input.GetMouseButtonUp(0))
                {
                   
                    if (manaFireCoroutine != null)
                    {
                        StopCoroutine(manaFireCoroutine);
                        manaFireCoroutine = null;
                    }
                }
                break;

            case SubWeaponType.ChargeBased:
                if (Input.GetMouseButtonDown(0))
                {
                    isCharging = true;
                    charged = false;
                    currentChargeTime = 0f;
                }
                if (isCharging)
                {
                    currentChargeTime += Time.deltaTime;
                    if (!charged && currentChargeTime >= equippedSubWeapon.requiredChargeTime)
                        charged = true;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    isCharging = false;
                    if (charged && CanUseSubWeapon())
                        UseSubWeapon();
                }
                break;
        }
    }


    public void EquipSubWeapon(SubWeaponData data)
    {
        equippedSubWeapon = data;
        upgradeComponent.Init(data);

        AmmoUIGroup.SetActive(data.weaponType == SubWeaponType.AmmoBased);
        ManaUIGroup.SetActive(data.weaponType == SubWeaponType.ManaBased);
        ChargeUIGroup.SetActive(data.weaponType == SubWeaponType.ChargeBased);

        follower.Init(data, 0f);
        follower.SetMainWeapon(followTarget, 0f);

        uiPanel.gameObject.SetActive(true);

        HideAllUI();


        if (data.weaponType == SubWeaponType.AmmoBased)
        {

            maxAmmo = data.maxAmmo;
            currentAmmo = maxAmmo;

      
        foreach (var ico in ammoIcons) Destroy(ico.gameObject);
        ammoIcons.Clear();
        for (int i = 0; i < maxAmmo; i++)
        {
            var img = Instantiate(ammoIconPrefab, uiPanel);
            ammoIcons.Add(img);
        }
    }
    

        else if (data.weaponType == SubWeaponType.ManaBased)
        {
            currentMana = maxMana;
            manaBar.gameObject.SetActive(true);
            manaBar.fillAmount = 1f;
        }
        else if (data.weaponType == SubWeaponType.ChargeBased)
        {
            isCharging = false;
            charged = false;
            currentChargeTime = 0f;

            chargeEmptyBar.gameObject.SetActive(true);
            chargeEmptyBar.fillAmount = 0f;
            chargeCompleteBar.gameObject.SetActive(false);
        }
    }

    bool CanUseSubWeapon()
    {
        if (currentCooldown > 0f) return false;
        switch (equippedSubWeapon.weaponType)
        {
            case SubWeaponType.AmmoBased:
                return currentAmmo > 0;
            case SubWeaponType.ManaBased:
                return currentMana >= equippedSubWeapon.manaCost;
            case SubWeaponType.ChargeBased:
                return charged;
            default:
                return true;
        }
    }

    public void UseSubWeapon()
    {
        //  쿨타임 세팅 (모든 타입)
        currentCooldown = equippedSubWeapon.cooldown;

        // 자원 소모
        if (equippedSubWeapon.weaponType == SubWeaponType.AmmoBased)
            currentAmmo--;
        else if (equippedSubWeapon.weaponType == SubWeaponType.ManaBased)
            currentMana = Mathf.Max(0f, currentMana - equippedSubWeapon.manaCost);

        // 발사 이펙트 / 투사체 분기
        if (equippedSubWeapon.rangeShape == SubWeaponRangeShape.ShortLine)
            UseLineEffectAtCursor();
        else if (equippedSubWeapon.rangeType == SubWeaponRangeType.Radial)
            UseForceEffectAtCursor();
        else
            ShootToNearestEnemy();

        // UI 즉시 갱신
        if (equippedSubWeapon.weaponType == SubWeaponType.AmmoBased)
            UpdateAmmoUI();
        else if (equippedSubWeapon.weaponType == SubWeaponType.ManaBased)
            UpdateManaUI();
        else if (equippedSubWeapon.weaponType == SubWeaponType.ChargeBased)
            UpdateChargeUI();

        // ChargeBased 발사 뒤 charged 플래그 초기화
        if (equippedSubWeapon.weaponType == SubWeaponType.ChargeBased)
            charged = false;
    }

    private IEnumerator ResetChargedAfterDelay(float delay)
    {
        
        yield return new WaitForSeconds(delay);
        charged = false;
    }

    private IEnumerator ContinuousManaFire()
    {
        // 마우스를 누르고 있고, 쿨다운/마나 조건이 되는 한
        while (Input.GetMouseButton(0) && CanUseSubWeapon())
        {
            UseSubWeapon();
            // 투사체 애니메이션/쿨다운과 동일한 간격으로 대기
            yield return new WaitForSeconds(equippedSubWeapon.cooldown);
        }
        manaFireCoroutine = null;
    }
    void HideAllUI()
    {
        // Ammo
        foreach (var ico in ammoIcons) ico.gameObject.SetActive(false);
        
        // Mana
        if (manaBar != null)
            manaBar.gameObject.SetActive(false);

        // Charge
        if (chargeEmptyBar != null)
            chargeEmptyBar.gameObject.SetActive(false);
        if (chargeCompleteBar != null)
            chargeCompleteBar.gameObject.SetActive(false);
    }


    private void UpdateAmmoUI()
    {
        for (int i = 0; i < ammoIcons.Count; i++)
            ammoIcons[i].sprite = (i < currentAmmo)
                ? ammoIconPrefab.sprite
                : usedAmmoSprite;
    }

    void HandleAmmoInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            currentAmmo = maxAmmo;
            Debug.Log("리로드 완료");
        }
    }
    void UpdateManaUI()
    {
        manaBar.fillAmount = currentMana / maxMana;
        manaBar.gameObject.SetActive(true);
    }

    void HandleManaInput()
    {
       
    }
    
     void UpdateChargeUI()
    {
        if (isCharging && !charged)
        {
            
            chargeEmptyBar.gameObject.SetActive(true);
            chargeEmptyBar.fillAmount = currentChargeTime / equippedSubWeapon.requiredChargeTime;
            chargeCompleteBar.gameObject.SetActive(false);
        }
        else if (charged)
        {
            
            chargeEmptyBar.gameObject.SetActive(false);
            chargeCompleteBar.gameObject.SetActive(true);
        }
        else
        {
            
            chargeEmptyBar.gameObject.SetActive(false);
            chargeCompleteBar.gameObject.SetActive(false);
        }
    }

    void HandleChargeInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isCharging = true;
            currentChargeTime = 0f;
            charged = false;
        }
        if (isCharging && Input.GetMouseButton(0))
        {
            currentChargeTime += Time.deltaTime;
            if (!charged && currentChargeTime >= equippedSubWeapon.requiredChargeTime)
                charged = true;
        }
        if (isCharging && Input.GetMouseButtonUp(0))
        {
            isCharging = false;
            if (charged && CanUseSubWeapon())
                UseSubWeapon();
            currentChargeTime = 0f;
            charged = false;
        }
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
        currentAmmo = maxAmmo;
        Debug.Log("리로드 시작");
    }

    private BaseMonster FindNearestAliveMonster(Vector3 from)
    {
        var monsters = FindObjectsOfType<BaseMonster>();
        BaseMonster nearest = null;
        float minDist = float.MaxValue;
        foreach (var m in monsters)
        {
            if (m.IsDead) continue;
            float d = Vector2.Distance(from, m.transform.position);
            if (d < minDist)
            {
                minDist = d;
                nearest = m;
            }
        }
        return nearest;
    }

    void UseForceEffectAtCursor()
    {
        // 커서 중심 위치
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0f;

        // 1) VFX Zone 생성
        if (equippedSubWeapon.ForceVisualPrefab != null)
        {
            var zone = Instantiate(
                equippedSubWeapon.ForceVisualPrefab,
                pos,
                Quaternion.identity
            );
            float dia = equippedSubWeapon.effectRadius * 2f;
            zone.transform.localScale = new Vector3(dia, dia, 1f);
            Destroy(zone, equippedSubWeapon.effectDuration);
        }

        // 2) OverlapCircleAll로 즉시 데미지 존
        int dmg = Mathf.RoundToInt(upgradeComponent.GetCurrentDamage());
        var hits = Physics2D.OverlapCircleAll(
            pos,
            equippedSubWeapon.effectRadius,
            monsterLayer
        );
        foreach (var col in hits)
        {
            if (col.TryGetComponent<BaseMonster>(out var m) && !m.IsDead)
            {
                m.TakeDamage(dmg, equippedSubWeapon);
                if (equippedSubWeapon.stunOnRadial)
                    m.Stun(equippedSubWeapon.stunDuration);
            }
        }
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
