using UnityEngine;

public class SubWeaponManager : MonoBehaviour
{
    public SubWeaponData equippedSubWeapon;

    private float currentCooldown = 0f;

    void Update()
    {
        if (currentCooldown > 0f)
            currentCooldown -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && CanUseSubWeapon())
        {
            UseSubWeapon();
        }
    }

    public bool CanUseSubWeapon()
    {
        return equippedSubWeapon != null && currentCooldown <= 0f;
    }

    public void UseSubWeapon()
    {
        currentCooldown = equippedSubWeapon.cooldown;

        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target.z = 0;

        GameObject prefab = equippedSubWeapon.projectilePrefab;
        GameObject projectile = Instantiate(prefab, transform.position, Quaternion.identity);

        SubProjectile sub = projectile.GetComponent<SubProjectile>();
        sub.Init(equippedSubWeapon, target);
    }
}
