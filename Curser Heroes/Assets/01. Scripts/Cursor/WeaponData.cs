using UnityEngine;
[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/WeaponData")]      //무기선택과  무기설명
public class WeaponData : ScriptableObject
{
    [Header("기본 정보")]
    public string weaponName;
    public Sprite weaponImage;
    public string weaponDesc;
    public int level;
    public int upgradeCost;
    
    [Header("공격력 관련")]
    public float baseDamage = 10;     
    public float damagePerLevel = 2f;  // 무기마다 다르게 설정 가능
    
    [Header("고정 스탯")]
    public float attackCooldown = 0.5f;   //쿨타임
    public float attackRange = 0.5f;      // 공격 범위

    [Header("목숨")]
    public int maxLives = 10; // 무기가 가질 수 있는 최대 목숨 수

    // 공격력 계산
    public float GetDamage(int level)      //무기 레벨에 따른 공격력 계산 함수
    {
        return baseDamage + damagePerLevel * level;    // 기본데미지 + (레벨당 상승 공격력 * 레벨)
    }

}
