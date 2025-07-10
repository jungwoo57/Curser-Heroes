using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    private List<SkillData> hasSkills;
    private List<OwnedWeapon> ownedWeapons;
    private List<OwnedSubWeapon> ownedSubWeapons;
    private OwnedWeapon mainEquipWeapon;
    private OwnedSubWeapon subEquipWeapon;
    private List<SkillData> selectedSkills;
    
    private int gold;
    private int jewel;
    private int bsetScore;
}
