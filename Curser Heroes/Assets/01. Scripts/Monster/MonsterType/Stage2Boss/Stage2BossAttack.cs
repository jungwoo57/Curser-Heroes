using UnityEngine;

public class Stage2BossAttack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            if (WeaponManager.Instance != null && !WeaponManager.Instance.isInvincible)
            {
                WeaponManager.Instance.TakeWeaponLifeBossDamage();
            }
        }
    }
}
