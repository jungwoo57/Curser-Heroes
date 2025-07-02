using UnityEngine;

public class ShortRangeProjectile : MonoBehaviour
{
    public float speed = 15f;
    public float maxDistance = 2f;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
      //Vector3 dir = (targetPosition - transform.position).normalized;
      //transform.position += dir * speed * Time.deltaTime;

        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            Monster m = other.GetComponent<Monster>();
          //ApplyDamage(m);
          //ApplyEffect(m);
            Destroy(gameObject);
        }
    }
}
