using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<SkillData> hasSkills;
    public List<OwnedWeapon> ownedWeapons;
    public List<OwnedSubWeapon> ownedSubWeapons;
    public OwnedWeapon mainEquipWeapon;
    public OwnedSubWeapon subEquipWeapon;
    public List<SkillData> selectedSkills;
    
    public int gold;
    public int jewel;
    public int bestScore;
}
