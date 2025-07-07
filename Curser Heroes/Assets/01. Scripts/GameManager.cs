using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }

            return instance;
        }
    }

    [Header("모든 무기 및 스킬")]
    public List<WeaponData> allMainWeapons; // 모든 무기 원본
    public List<SubWeaponData> allSubWeapons; // 모든 보조 무기 원본
    public List<SkillData> allSkills;// 모든 스킬
    public IReadOnlyList<WeaponData> hasPartner => _hasPartner; // 다른 파일에서 보유 동료 가져오기(수정 불가)

    [SerializeField] private List<WeaponData> _hasPartner; // 보유 동료
    [SerializeField] private List<SkillData> _hasSkills = new List<SkillData>();
    
    [Header("보유 중인 무기 및 스킬")]
    [SerializeField] public List<OwnedWeapon> ownedWeapons; // 소유 메인 무기
    [SerializeField] public List<OwnedSubWeapon> ownedSubWeapons; // 소유 보조 무기
    
    public List<SkillData> hasSkills => _hasSkills;
    public List<SkillData> skillPool = new List<SkillData>();
    
    [Header("장착 및 선택한 스킬")]
    public OwnedWeapon mainEquipWeapon;
    public OwnedSubWeapon subEquipWeapon;
    public List<SkillData> selectSkills = new List<SkillData>(); // 플레이어가 선택한 스킬 12개 //선택한 스킬(스테이지에 등장할 스킬), 스킬 갯수가 정해져있어서 배열로 변경도 고려

    [Header("기타 데이터")]
    [SerializeField] private int gold = 9999;
    private int jewel = 0;
    public int bestScore;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            _hasSkills = new List<SkillData>(allSkills);
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

    public void UnlockWeapon(WeaponData weaponData) //무기 해금 시 사용
    {
        // 이미 보유중인 무기면 적용 안시킬 지는 무기 해금 코드 보고 결정
        ownedWeapons.Add(new OwnedWeapon(weaponData)); // 코드 구조 보고 보조무기 주무기 얻는 법 바꾸기
    }

    public void UnlockWeapon(SubWeaponData weaponData) //무기 해금 시 사용
    {
        // 이미 보유중인 무기면 적용 안시킬 지는 무기 해금 코드 보고 결정
        ownedSubWeapons.Add(new OwnedSubWeapon(weaponData)); // 코드 구조 보고 보조무기 주무기 얻는 법 바꾸기
    }
    
    public void EquipWeapon(OwnedWeapon equipData)
    {
        if (equipData == null)
        {
            Debug.Log("데이터 없음");
            return;
        }

        mainEquipWeapon = equipData; // 메인 무기 장착만 작성 추후 보조 동료 추가
    }

    public void EquipWeapon(OwnedSubWeapon equipData)
    {
        if (equipData == null)
        {
            Debug.Log("데이터 없음");
            return;
        }

        subEquipWeapon = equipData; // 메인 무기 장착만 작성 추후 보조 동료 추가
    }

    public void EquipSkill(SkillData[] skilldatas)
    {
        selectSkills.Clear();
        for (int i = 0; i < skilldatas.Length; i++)
        {
            selectSkills.Add(skilldatas[i]);
        }
        skillPool = new List<SkillData>(selectSkills);
    }

    public void UpgradeWeapon(WeaponData data)
    {
        int index = ownedWeapons.FindIndex(w => w.data.weaponName == data.weaponName);
        if (index >= 0)
        {
            gold -= data.upgradeCost;
            ownedWeapons[index].level++;
            Debug.Log(ownedWeapons[index].data.name + "업그레이드 완료" + ownedWeapons[index].level);
        }
        else
        {
            Debug.Log("해당 데이터 없음");
        }
    }
    
    public void UpgradeWeapon(SubWeaponData data)
    {
        int index = ownedSubWeapons.FindIndex(w => w.data.weaponName == data.weaponName);
        if (index >= 0)
        {
            gold -= data.upgradeCost;
            ownedSubWeapons[index].level++;
            Debug.Log(ownedSubWeapons[index].data.name + "업그레이드 완료" + ownedSubWeapons[index].level);
        }
        else
        {
            Debug.Log("해당 데이터 없음");
        }
    }

    public void UnlockSkill(SkillData skilldata)
    {
        _hasSkills.Add(skilldata);
    }
}
