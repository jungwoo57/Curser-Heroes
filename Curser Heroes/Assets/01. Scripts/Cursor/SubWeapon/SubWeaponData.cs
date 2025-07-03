using UnityEngine;

public enum SubWeaponRangeShape { LongProjectile, ShortLine, ShortCircle }

[CreateAssetMenu(fileName = "SubWeaponData", menuName = "Weapon/Create Sub Weapon")]
public class SubWeaponData : ScriptableObject
{
    [Header("기본 정보")]
    public string weaponName;
    public Sprite weaponImage;
    public string weaponDesc;

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

    public float GetDamage(int level = 0)
    {
        return baseDamage + damagePerLevel * level;
    }
   
    
}
