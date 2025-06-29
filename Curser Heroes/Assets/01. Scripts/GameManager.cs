using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    

    [SerializeField] private List<WeaponData> _hasMainWeapon; // 보유 주무기
    public IReadOnlyList<WeaponData> hasMainWeapon => _hasMainWeapon;     // 다른 파일에서 보유 주무기 가져오기(수정 불가)
 
    [SerializeField] private List<WeaponData> _hasSubWeapon; // 보유 보조무기
    public IReadOnlyList<WeaponData> hasSubWeapon => _hasSubWeapon;     // 다른 파일에서 보유 보조 무기 가져오기(수정 불가)
    
    [SerializeField] private List<WeaponData> _hasPartner; // 보유 동료
    public IReadOnlyList<WeaponData> hasPartner => _hasPartner;     // 다른 파일에서 보유 동료 가져오기(수정 불가)
    
     
    private int gold = 0;
    private int jewel = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void AddGold(int amount)
    {
        gold += amount;
        UIManager.Instance.battleUI.TextUpdate();
        Debug.Log($"골드 획득: {amount} / 총 골드: {gold}");
    }

    public void AddJewel(int amount)
    {
        jewel += amount;
        UIManager.Instance.battleUI.TextUpdate();
        Debug.Log($"쥬얼 획득: {amount} / 총 쥬얼: {jewel}");
    }
    public int GetGold()
    {
        return gold;
    }

    public int GetJewel()
    {
        return jewel;
    }

    public void UnlockWeapon(WeaponData weaponData)  //무기 해금 시 사용
    {
        // 이미 보유중인 무기면 적용 안시킬 지는 무기 해금 코드 보고 결정
        _hasMainWeapon.Add(weaponData);              // 코드 구조 보고 보조무기 주무기 얻는 법 바꾸기
    }
}
