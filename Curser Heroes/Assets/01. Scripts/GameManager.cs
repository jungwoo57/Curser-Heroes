using System;
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
    public List<PartnerData> allPartners;
    public List<SkillData> allSkills;// 모든 스킬
    
    [SerializeField] private List<SkillData> _hasSkills = new List<SkillData>();
    
    [Header("보유 중인 무기 및 동료")]
    [SerializeField] public List<OwnedWeapon> ownedWeapons; // 소유 메인 무기
    [SerializeField] public List<OwnedSubWeapon> ownedSubWeapons; // 소유 보조 무기
    [SerializeField] public List<OwnedPartner> ownedPartners;
    public List<SkillData> hasSkills => _hasSkills;
    public List<SkillData> skillPool = new List<SkillData>();
    
    [Header("장착 및 선택한 스킬")]
    public OwnedWeapon mainEquipWeapon;
    public OwnedSubWeapon subEquipWeapon;
    public List<SkillData> selectSkills = new List<SkillData>(); // 플레이어가 선택한 스킬 12개 //선택한 스킬(스테이지에 등장할 스킬), 스킬 갯수가 정해져있어서 배열로 변경도 고려
    public OwnedPartner equipPartner;
    
    [Header("기타 데이터")]
    [SerializeField] private int gold = 9999;
    [SerializeField]private int jewel = 0;
    public int bestScore;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            _hasSkills = new List<SkillData>(allSkills);
            skillPool = new List<SkillData>(allSkills); //테스트 편의를 위해 잠시 모든 스킬의 데이터를 가져와 사용 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (ownedWeapons != null)
        {
            mainEquipWeapon = ownedWeapons[0];
        }

        if (ownedSubWeapons != null)
        {
            subEquipWeapon = ownedSubWeapons[0];
        }

        if (ownedPartners != null)
        {
            equipPartner = ownedPartners[0];
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
        if (weaponData.unlockCost <= jewel)
        {
            jewel -= weaponData.unlockCost;
            ownedWeapons.Add(new OwnedWeapon(weaponData));
        }
    }

    public void UnlockWeapon(SubWeaponData weaponData) //무기 해금 시 사용
    {

        if (weaponData.unlockCost <= jewel)
        {
            jewel -= weaponData.unlockCost;
            ownedSubWeapons.Add(new OwnedSubWeapon(weaponData));
        }
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

    public void EquipPartner(OwnedPartner equipData)
    {
        if (equipData == null)
        {
            return;
        }
        equipPartner = equipData;
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
        if (index >= 0 && ownedWeapons[index].level<10)
        {
            gold -= data.upgradeCost[ownedWeapons[index].level];
            ownedWeapons[index].level++;
            Debug.Log(ownedWeapons[index].data.name + "업그레이드 완료" + ownedWeapons[index].level);
        }
        else
        {
            Debug.Log("해당 데이터 없거나 레벨이 만랩");
        }
    }
    
    public void UpgradeWeapon(SubWeaponData data)
    {
        int index = ownedSubWeapons.FindIndex(w => w.data.weaponName == data.weaponName);
        if (index >= 0)
        {
            gold -= data.upgradeCost[ownedSubWeapons[index].level];
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
        if (skilldata.unlockCost <= jewel)
        {
            jewel -= skilldata.unlockCost;
            _hasSkills.Add(skilldata);
        }
    }

    public void UpgradePartner(PartnerData data)
    {
        int index = ownedPartners.FindIndex(w => w.data.partnerName == data.partnerName);
        if (index >= 0)
        {
            gold -= data.upgradeCost[ownedPartners[index].level];
            ownedPartners[index].level++;
        }
    }
    public void UnlockPartner(PartnerData partnerData)
    {
        if (partnerData.unlockCost <= jewel)
        {
            jewel -= partnerData.unlockCost;
            ownedPartners.Add(new OwnedPartner(partnerData));
        }
            
    }
        
    [ContextMenu("TestSave")]
    public void Save()
    {
        if (!SaveLoadManager.instance) return;
        SaveData data = new SaveData();
        data.hasSkills = _hasSkills;
        data.ownedWeapons = ownedWeapons;
        data.ownedSubWeapons = ownedSubWeapons;
        data.mainEquipWeapon = mainEquipWeapon;
        data.subEquipWeapon = subEquipWeapon;
        data.selectedSkills = selectSkills;
        data.gold = gold; 
        data.jewel = jewel;
        data.bestScore = bestScore;

        SaveLoadManager.instance.Save(data);
    }

    [ContextMenu("Load")]
    public void Load()
    {
        SaveData loadData = new SaveData();
        loadData = SaveLoadManager.instance.Load();
        if (loadData == null) return;
        _hasSkills = loadData.hasSkills;
        ownedWeapons = loadData.ownedWeapons;
        ownedSubWeapons = loadData.ownedSubWeapons;
        mainEquipWeapon = loadData.mainEquipWeapon;
        subEquipWeapon = loadData.subEquipWeapon;
        selectSkills =  loadData.selectedSkills;
        gold = loadData.gold;
        jewel = loadData.jewel;
        bestScore = loadData.bestScore;
    }

    public void OnApplicationQuit()      //마을에서 게임 껏을 시 자동 저장
    {
        if (SaveLoadManager.instance != null)
        {
            Save();
        }
    }
}
