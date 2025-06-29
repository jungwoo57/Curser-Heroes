using UnityEngine;

[CreateAssetMenu(fileName = "SubWeaponData", menuName = "Weapon/Create Sub Weapon")]
public class SubWeaponData : ScriptableObject
{
    [Header("기본 정보")]
    public string weaponName;    

    [Header("기본 스탯")]
    public float baseDamage = 1f;
    public float damagePerLevel = 1f;

    [Header("무기 타입 및 효과")]
    public SubWeaponType weaponType;
    public SubWeaponEffect effectType;

    public float GetDamage(int level)
    {
        return baseDamage + damagePerLevel * level;
    }
}
