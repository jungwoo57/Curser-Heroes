using UnityEngine;

public class SubWeaponUpgrade : MonoBehaviour
{
    public SubWeaponData currentSubWeapon;         //강화 레벨, 데미지 증가 
    public int weaponLevel = 0;

    // 보조무기 설정
    public void Init(SubWeaponData subWeaponData)         //무기 설정시 초기화
    {
        currentSubWeapon = subWeaponData;
        weaponLevel = 0;
    }

    // 강화 함수
    public void Upgrade()      
    {
        weaponLevel++;
        float newDamage = currentSubWeapon.GetDamage(weaponLevel);
        Debug.Log($"[보조무기 강화] {currentSubWeapon.weaponName} 레벨: {weaponLevel}, 데미지: {newDamage}");
    }

    public float GetCurrentDamage()      
    {
        return currentSubWeapon.GetDamage(weaponLevel);
    }
}
