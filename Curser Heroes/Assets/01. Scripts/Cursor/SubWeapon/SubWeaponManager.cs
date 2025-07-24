using System.Collections.Generic;
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

    // 장탄형 UI
    public Image ammoIconPrefab;
    public Sprite usedAmmoSprite;
    private List<Image> ammoIcons = new List<Image>();

    // 마나형 UI
    public Image manaBar;
    private float maxMana = 100f;
    private float currentMana;

    // 차징형 UI
    public Image chargeBar;
    public Image chargeCompleteIcon;
    private float currentChargeTime;
    private bool isCharging;
    private bool charged;

    // 공통 상태
    private int maxAmmo;
    private int currentAmmo;
    private float currentCooldown;


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
        uiPanel.gameObject.SetActive(false);
        HideAllUI();

        if (equippedSubWeapon != null)
            EquipSubWeapon(equippedSubWeapon);
    }

    void Update()
    {
        if (equippedSubWeapon == null) return;
        HideAllUI();

        if (currentCooldown > 0f)
            currentCooldown -= Time.deltaTime;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(followTarget.position);
        uiPanel.position = screenPos + new Vector3(0, -30, 0);
        switch (equippedSubWeapon.weaponType)
        {
            case SubWeaponType.AmmoBased:
                foreach (var ico in ammoIcons)
                    ico.gameObject.SetActive(true);
                UpdateAmmoUI();
                break;
            case SubWeaponType.ManaBased:
                manaBar.gameObject.SetActive(true);
                UpdateManaUI();
                break;
            case SubWeaponType.ChargeBased:
                chargeBar.gameObject.SetActive(true);
                UpdateChargeUI();
                break;
        }

        // 입력 처리 (클릭형 / 충전형)
        if (equippedSubWeapon.weaponType == SubWeaponType.ChargeBased)
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
        else if (Input.GetMouseButtonDown(0) && CanUseSubWeapon())
        {
            UseSubWeapon();
        }

        // 마나 회복
        if (equippedSubWeapon.weaponType == SubWeaponType.ManaBased)
            currentMana = Mathf.Min(100f, currentMana + equippedSubWeapon.manaCost * Time.deltaTime);

        // 탄약 리로드
        if (equippedSubWeapon.weaponType == SubWeaponType.AmmoBased && Input.GetKeyDown(KeyCode.R))
            StartReloading();
    }

    public void EquipSubWeapon(SubWeaponData data)
    {
        equippedSubWeapon = data;
        upgradeComponent.Init(data);

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

            chargeBar.gameObject.SetActive(true);
            chargeBar.fillAmount = 0f;
            chargeCompleteIcon.gameObject.SetActive(false);
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

    void UseSubWeapon()
    {
        currentCooldown = equippedSubWeapon.cooldown;

        if (equippedSubWeapon.weaponType == SubWeaponType.AmmoBased)
            currentAmmo--;
        else if (equippedSubWeapon.weaponType == SubWeaponType.ManaBased)
            currentMana = Mathf.Max(0f, currentMana - equippedSubWeapon.manaCost);

        // 발사 효과
        if (equippedSubWeapon.rangeShape == SubWeaponRangeShape.ShortLine)
            UseLineEffectAtCursor();
        else if (equippedSubWeapon.rangeType == SubWeaponRangeType.Radial)
            UseForceEffectAtCursor();
        else
            ShootToNearestEnemy();

       
        if (equippedSubWeapon.weaponType == SubWeaponType.AmmoBased)
            UpdateAmmoUI();
        else if (equippedSubWeapon.weaponType == SubWeaponType.ManaBased)
            UpdateManaUI();
    }

    void HideAllUI()
    {
        // Ammo
        foreach (var ico in ammoIcons) ico.gameObject.SetActive(false);
        // Mana
        manaBar.gameObject.SetActive(false);
        // Charge
        chargeBar.gameObject.SetActive(false);
        chargeCompleteIcon.gameObject.SetActive(false);
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
    }

    void HandleManaInput()
    {
       
    }
    
    private void UpdateChargeUI()
    {
        if (isCharging)
        {
            chargeBar.fillAmount = Mathf.Clamp01(
                currentChargeTime / equippedSubWeapon.requiredChargeTime
            );
            chargeCompleteIcon.gameObject.SetActive(false);
        }
        else if (charged)
        {
            chargeBar.fillAmount = 1f;
            chargeCompleteIcon.gameObject.SetActive(true);
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
