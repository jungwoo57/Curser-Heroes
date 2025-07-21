using UnityEngine;

public enum SubWeaponRangeShape { LongProjectile, ShortLine, ShortCircle }

[CreateAssetMenu(fileName = "SubWeaponData", menuName = "Weapon/Create Sub Weapon")]
public class SubWeaponData : ScriptableObject
{
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



    public float GetDamage(int level = 0)
    {
        float bonusFromSkill = SkillManager.Instance != null ? SkillManager.Instance.BonusSubWeaponDamage : 0f;
        float totalDamage = baseDamage + damagePerLevel * level + bonusFromSkill;
        Debug.Log($"[SubWeaponData] 계산된 데미지: {baseDamage} + {damagePerLevel}*{level} + {bonusFromSkill} = {totalDamage}");
        return totalDamage;
    }


}
