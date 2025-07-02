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
 
    [SerializeField] private List<SubWeaponData> _hasSubWeapon; // 보유 보조무기
    public IReadOnlyList<SubWeaponData> hasSubWeapon => _hasSubWeapon;     // 다른 파일에서 보유 보조 무기 가져오기(수정 불가)
    
    [SerializeField] private List<WeaponData> _hasPartner; // 보유 동료
    public IReadOnlyList<WeaponData> hasPartner => _hasPartner;     // 다른 파일에서 보유 동료 가져오기(수정 불가)

    [SerializeField] private List<SkillData> _hasSkills;  // 플레이어가 해금하여 보유하고 있는 스킬
    public IReadOnlyList<SkillData> hasSkills => _hasSkills;
    
    public WeaponData mainEquipWeapon;
    public SubWeaponData subEquipWeapon;
    public List<SkillData> selectSkills;    //선택한 스킬(스테이지에 등장할 스킬), 스킬 갯수가 정해져있어서 배열로 변경도 고려
    
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
    
    public void EquipWeapon(WeaponData equipData)
    {
        if (equipData == null)
        {
            Debug.Log("데이터 없음");
            return;
        }

        mainEquipWeapon= equipData;       // 메인 무기 장착만 작성 추후 보조 동료 추가
    }
    
    public void EquipWeapon(SubWeaponData equipData)
    {
        if (equipData == null)
        {
            Debug.Log("데이터 없음");
            return;
        }

        subEquipWeapon= equipData;       // 메인 무기 장착만 작성 추후 보조 동료 추가
    }

    public void EquipSkill(SkillData[] skilldatas)
    {
        selectSkills.Clear();                        //기존 스킬 초기화
        for (int i = 0; i < skilldatas.Length; i++)
        {
            selectSkills.Add(skilldatas[i]);
        }
    }

    public void UpgradeWeapon(WeaponData data)
    {
        int index = _hasMainWeapon.FindIndex(w => w.name == data.name);
        if (index >= 0)
        {
            _hasMainWeapon[index] = data;
            Debug.Log(_hasMainWeapon[index].name+ "업그레이드 완료" + _hasMainWeapon[index].level);
        }
        else
        {
            Debug.Log("해당 데이터 없음");
        }
    }
}
