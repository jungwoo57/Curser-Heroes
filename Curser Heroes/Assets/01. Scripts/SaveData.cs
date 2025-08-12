using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<SkillData> unlockedSkills;
    public List<SkillData> selectedSkills;
    public List<OwnedWeapon> ownedWeapons;
    public List<OwnedSubWeapon> ownedSubWeapons;
    public List<OwnedPartner> ownedPartners;
    public OwnedWeapon mainEquipWeapon;
    public OwnedSubWeapon subEquipWeapon;

    public List<string> mainWeaponName  = new List<string>();
    public List<string> subWeaponName  = new List<string>();
    public List<string> partnerName  = new List<string>();
    
    public int stage1bestWave;
    public int stage2bestWave;
    public int stage3bestWave;
    
    public int gold;
    public int jewel;

    public bool useStage;
    public bool useForge;
    public bool useLab;
    public bool useBar;
    public bool useSkillList;
    public bool useWeaponSelectList;
}
