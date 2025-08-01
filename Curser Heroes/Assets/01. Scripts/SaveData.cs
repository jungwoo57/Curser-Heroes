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
    public OwnedWeapon mainEquipWeapon;
    public OwnedSubWeapon subEquipWeapon;
    
    public int gold;
    public int jewel;
    public int bestScore;
}
