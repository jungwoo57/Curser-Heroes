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
    
    [SerializeField] private List<SkillData> defaultSkills = new List<SkillData>();  // 처음부터 제공되는 스킬
    [SerializeField] private List<SkillData> unlockedSkills = new List<SkillData>(); // 해금한 스킬

    [Header("보유 중인 무기 및 동료")]
    [SerializeField] public List<OwnedWeapon> ownedWeapons; // 소유 메인 무기
    [SerializeField] public List<OwnedSubWeapon> ownedSubWeapons; // 소유 보조 무기
    [SerializeField] public List<OwnedPartner> ownedPartners;
    public List<SkillData> skillPool = new List<SkillData>();
    
    [Header("장착 및 선택한 스킬")]
    public OwnedWeapon mainEquipWeapon;
    public OwnedSubWeapon subEquipWeapon;
    public List<SkillData> selectSkills = new List<SkillData>(); // 플레이어가 선택한 스킬 12개 //선택한 스킬(스테이지에 등장할 스킬), 스킬 갯수가 정해져있어서 배열로 변경도 고려
    public OwnedPartner equipPartner;
    
    [Header("기타 데이터")]
    [SerializeField] private int gold = 9999;
    [SerializeField]private int jewel = 0;
    
    [Header("튜토리얼 체크용")]
    public bool useStage;
    public bool useForge;
    public bool useLab;
    public bool useBar;

    public event Action OnGoldChanged;
    public event Action OnJewelChanged;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            defaultSkills = allSkills.FindAll(skill => skill.isDefaultSkill);
            unlockedSkills = new List<SkillData>(); // 저장된 스킬은 Load에서 복원
            skillPool = new List<SkillData>(HasSkills); // 선택 풀은 제공 + 해금 스킬에서만
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
        if (UIManager.Instance != null)
        {
            UIManager.Instance.battleUI.TextUpdate();
        }

        OnGoldChanged?.Invoke();
        Debug.Log($"골드 획득: {amount} / 총 골드: {gold}");
    }

    public void AddJewel(int amount)
    {
        jewel += amount;
        if (UIManager.Instance != null)
        {
            UIManager.Instance.battleUI.TextUpdate();
        }

        OnJewelChanged?.Invoke();
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
            AddJewel(- weaponData.unlockCost);
            ownedWeapons.Add(new OwnedWeapon(weaponData));
        }

        Save();
    }

    public void UnlockWeapon(SubWeaponData weaponData) //무기 해금 시 사용
    {

        if (weaponData.unlockCost <= jewel)
        {
            AddJewel(- weaponData.unlockCost);
            ownedSubWeapons.Add(new OwnedSubWeapon(weaponData));
        }
        Save();
    }
    
    public void EquipWeapon(OwnedWeapon equipData)
    {
        if (equipData == null)
        {
            Debug.Log("데이터 없음");
            return;
        }

        mainEquipWeapon = equipData; // 메인 무기 장착만 작성 추후 보조 동료 추가
        Save();
    }

    public void EquipWeapon(OwnedSubWeapon equipData)
    {
        if (equipData == null)
        {
            Debug.Log("데이터 없음");
            return;
        }

        subEquipWeapon = equipData; // 메인 무기 장착만 작성 추후 보조 동료 추가
        Save();
    }

    public void EquipPartner(OwnedPartner equipData)
    {
        if (equipData == null)
        {
            return;
        }
        equipPartner = equipData;
        Save();
    }

    public void EquipSkill(SkillData[] skilldatas)
    {
        selectSkills.Clear();
        for (int i = 0; i < skilldatas.Length; i++)
        {
            selectSkills.Add(skilldatas[i]);
        }
        skillPool = new List<SkillData>(selectSkills);
        Save();
    }

    public void UpgradeWeapon(WeaponData data)
    {
        int index = ownedWeapons.FindIndex(w => w.data.weaponName == data.weaponName);
        if (index >= 0 && ownedWeapons[index].level<10)
        {
            //gold -= data.upgradeCost[ownedWeapons[index].level];
            AddGold(-data.upgradeCost[ownedWeapons[index].level]);
            ownedWeapons[index].level++;
            Debug.Log(ownedWeapons[index].data.name + "업그레이드 완료" + ownedWeapons[index].level);
        }
        else
        {
            Debug.Log("해당 데이터 없거나 레벨이 만랩");
        }
        Save();
    }
    
    public void UpgradeWeapon(SubWeaponData data)
    {
        int index = ownedSubWeapons.FindIndex(w => w.data.weaponName == data.weaponName);
        if (index >= 0)
        {
            AddGold(-data.upgradeCost[ownedSubWeapons[index].level]);
            ownedSubWeapons[index].level++;
            Debug.Log(ownedSubWeapons[index].data.name + "업그레이드 완료" + ownedSubWeapons[index].level);
        }
        else
        {
            Debug.Log("해당 데이터 없음");
        }
        Save();
    }


    public void UnlockSkill(SkillData skillData)
    {
        if (!unlockedSkills.Contains(skillData))
        {
            AddJewel(-skillData.unlockCost);
            unlockedSkills.Add(skillData);

           
            if (!HasSkills.Contains(skillData))
            {
                if(jewel >= skillData.unlockCost)
                {
                    AddJewel(-skillData.unlockCost);
                    HasSkills.Add(skillData);
                }
            }
            

            Save();
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
        Save();
    }
    public void UnlockPartner(PartnerData partnerData)
    {
        if (partnerData.unlockCost <= jewel)
        {
            jewel -= partnerData.unlockCost;
            ownedPartners.Add(new OwnedPartner(partnerData));
        }
        Save();
            
    }
    public List<SkillData> HasSkills
    {
        get
        {
            var all = new List<SkillData>(defaultSkills);
            all.AddRange(unlockedSkills);
            return all;
        }
    }

    [ContextMenu("TestSave")]
    public void Save()
    {
        if (!SaveLoadManager.instance) return;
        SaveData data = new SaveData();
        data.unlockedSkills = unlockedSkills;
        data.selectedSkills = selectSkills;
        data.ownedWeapons = ownedWeapons;
        data.ownedSubWeapons = ownedSubWeapons;
        data.mainEquipWeapon = mainEquipWeapon;
        data.subEquipWeapon = subEquipWeapon;
        data.selectedSkills = selectSkills;
        data.gold = gold; 
        data.jewel = jewel;
        data.stage1bestWave = StageManager.Instance.bestWave[0];
        data.stage1bestWave = StageManager.Instance.bestWave[1];
        data.stage1bestWave = StageManager.Instance.bestWave[2];
        SaveLoadManager.instance.Save(data);
    }

    [ContextMenu("Load")]
    public void Load()
    {
        SaveData loadData = new SaveData();
        loadData = SaveLoadManager.instance.Load();
        if (loadData == null) return;
        unlockedSkills = loadData.unlockedSkills ?? new List<SkillData>();
        selectSkills = loadData.selectedSkills ?? new List<SkillData>();
        skillPool = new List<SkillData>(HasSkills);
        ownedWeapons = loadData.ownedWeapons;
        ownedSubWeapons = loadData.ownedSubWeapons;
        mainEquipWeapon = loadData.mainEquipWeapon;
        subEquipWeapon = loadData.subEquipWeapon;
        selectSkills =  loadData.selectedSkills;
        gold = loadData.gold;
        jewel = loadData.jewel;
        StageManager.Instance.bestWave[0] = loadData.stage1bestWave;
        StageManager.Instance.bestWave[1] = loadData.stage2bestWave;
        StageManager.Instance.bestWave[2] = loadData.stage3bestWave;
        
        //bestScore = loadData.bestScore;
    }


    public void OnApplicationQuit()      //마을에서 게임 껏을 시 자동 저장
    {
        if (SaveLoadManager.instance != null)
        {
            Save();
        }
    }
}
