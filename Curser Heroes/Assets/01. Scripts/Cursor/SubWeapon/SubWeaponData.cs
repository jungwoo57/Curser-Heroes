using UnityEngine;

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

    [Header("무기 타입 및 효과")]
    public SubWeaponType weaponType;
    public SubWeaponEffect effectType;

    public GameObject projectilePrefab;

    public float GetDamage(int level = 0)
    {
        return baseDamage + damagePerLevel * level;
    }
}
