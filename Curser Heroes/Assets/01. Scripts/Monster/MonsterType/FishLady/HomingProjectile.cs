using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public float speed = 5f;
    public float turnSpeed = 200f; // 회전 속도 (더 크면 빠르게 방향 전환)
    private Transform target;
    private int damage;

    public void Initialize(Transform targetTransform, int dmg)
    {
        target = targetTransform;
        damage = dmg;
        Destroy(gameObject, 3f); // 수명 제한
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 회전 (선택사항)
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Quaternion.Euler(0, 0, angle),
            turnSpeed * Time.deltaTime
        );

        // 이동
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == target)
        {
            // 여기서 데미지 처리 등 원하는 행동 추가
            Debug.Log("유도 투사체가 타겟과 충돌!");
            Destroy(gameObject);
        }
    }
}
