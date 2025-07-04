using UnityEngine;

public class LongRangeProjectile : SubProjectile
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector2 dir = (targetPosition - transform.position).normalized;
        float speed = SubWeaponUtils.GetSpeed(weaponData.speed);
        rb.velocity = dir * speed;

        // 회전 유무
        if (weaponData.rotateWithDirection)
            transform.right = dir;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            Monster monster = other.GetComponent<Monster>();

            if (monster != null)
            {
                ApplyDamage(monster);
                ApplyEffect(monster);
            }

            Destroy(gameObject);
        }
    }
}
