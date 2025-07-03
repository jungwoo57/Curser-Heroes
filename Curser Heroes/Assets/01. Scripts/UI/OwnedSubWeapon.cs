using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnedSubWeapon
{
    public SubWeaponData data;
    public int level;
    public bool bookMark;
    
    public OwnedSubWeapon(SubWeaponData weaponData)
    {
        data = weaponData;
    }
    public float levelDamage => data.baseDamage + data.damagePerLevel * level;
    
    
}
