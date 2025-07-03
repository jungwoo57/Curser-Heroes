[System.Serializable]
public class OwnedWeapon 
{
    public WeaponData data;
    public int level;
    public bool bookMark;
    
    public OwnedWeapon(WeaponData weaponData)
    {
        data = weaponData;
    }
    public float levelDamage => data.baseDamage + data.damagePerLevel * level;
}
