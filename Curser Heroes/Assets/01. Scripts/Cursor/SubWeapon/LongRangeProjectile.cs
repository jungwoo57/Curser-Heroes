using UnityEngine;

public class LongRangeProjectile : SubProjectile
{
    public float speed = 10f;

    void Update()
    {
        Vector3 dir = (targetPosition - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            Monster m = other.GetComponent<Monster>();
            ApplyDamage(m);
            ApplyEffect(m);
            Destroy(gameObject);
        }
    }
}
