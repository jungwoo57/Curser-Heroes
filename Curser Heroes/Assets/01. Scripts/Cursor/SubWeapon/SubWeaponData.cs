using UnityEngine;

[CreateAssetMenu(fileName = "SubWeaponData", menuName = "Weapon/Create Sub Weapon")]
public class SubWeaponData : ScriptableObject
{
    [Header("사운드")]
    [Tooltip("이 보조무기를 사용할 때 재생할 사운드")]
    public AudioClip useSound;

    [Tooltip("useSound 재생 볼륨 (0~1)")]
    [Range(0f, 1f)]
    public float useSoundVolume = 1f;

    [Header("기본 정보")]
    public string weaponName;
    public Sprite weaponSprite;
    public string weaponDesc;
    public int[] upgradeCost;
    public int unlockCost;

    [Header("기본 스탯")]
    public float baseDamage = 1f;
    public float damagePerLevel = 1f;
    public float cooldown = 0.5f;

    [Header("보조무기 특성")]
    public SubWeaponEffect effect;
    public SubWeaponType weaponType;
    public SubWeaponRangeShape rangeShape;
    public SubProjectileSpeed speed = SubProjectileSpeed.Medium;
    public bool rotateWithDirection = false;

    [Header("투사체 설정")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public float projectileMaxDistance = 8f;
    public SubWeaponRangeType rangeType;

    [Header("장탄형 무기 설정")]
    public int maxAmmo;
    public float reloadTime;

    [Header("마나형 무기 설정")]
    public float manaCost;

    [Header("충전형 무기 설정")]
    public float requiredChargeTime;

    [Header("애니메이션 이펙트")]
    public GameObject ForceVisualPrefab;

    [Header("라인 이펙트 설정")]
    public GameObject LineVisualPrefab;
    public float effectWidth = 1.5f;
    public float effectDuration = 0.2f;

    [Header("범위형(Radial) 이펙트 반경")]
    public float effectRadius = 3f;

    [Header("상태이상 효과")]
    public SubWeaponEffect subeffect;

    [Tooltip("화상 효과일 때, 초당 데미지")]
    public int burnDamagePerSecond;

    [Tooltip("화상 효과 지속 시간")]
    public float burnDuration;

    [Tooltip("스턴 효과일 때, 스턴 지속 시간")]
    public float stunDuration;

    public float GetDamage(int level = 0)
    {
        float bonusFromSkill = SkillManager.Instance != null
            ? SkillManager.Instance.BonusSubWeaponDamage
            : 0f;
        float totalDamage = baseDamage + damagePerLevel * level + bonusFromSkill;
        Debug.Log($"[SubWeaponData] 계산된 데미지: {totalDamage}");
        return totalDamage;
    }
}
