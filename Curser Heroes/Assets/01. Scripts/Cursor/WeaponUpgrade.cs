using UnityEngine;

public class WeaponUpgrade : MonoBehaviour
{
    public WeaponData currentWeapon;           //무기 정보 확인
    public int weaponLevel = 0;                 //현재 무기 레벨 확인
    
    public void Init(WeaponData weaponData)     //무기 장착시 초기화
    {
        currentWeapon = weaponData;          
        weaponLevel = GameManager.Instance.mainEquipWeapon.level;                       //무기 초기 레벨은 0
    }

    public void Upgrade()             //무기 강화시 호출
    {
        weaponLevel++;                //무기레벨 1 증가
        float damage = currentWeapon.GetDamage(weaponLevel);   //레벨과 함께 공격력 확인
        Debug.Log($"{currentWeapon.weaponName} 레벨 {weaponLevel} 강화됨 → 공격력: {damage}");
    }
}
