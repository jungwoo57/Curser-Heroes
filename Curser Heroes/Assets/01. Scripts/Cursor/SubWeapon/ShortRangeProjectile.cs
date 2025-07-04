using UnityEngine;

public class ShortRangeProjectile : SubProjectile
{
    public float maxDistance = 2f;
    private Vector3 startPosition;
    private Rigidbody2D rb;

    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();

        Vector2 dir = (targetPosition - transform.position).normalized;
        float speed = SubWeaponUtils.GetSpeed(weaponData.speed);
        rb.velocity = dir * speed;

       
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, startPosition) >= maxDistance)
        {
            Destroy(gameObject);
        }
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
