using UnityEngine;

public class MeteorSkill : MonoBehaviour
{
    private int damage;
    private float fallSpeed = 10f;
    private Vector3 targetPosition;

    public void Init(int dmg, Vector3 targetPos)
    {
        damage = dmg;
        targetPosition = targetPos;
        // 시작 위치는 targetPosition의 오른쪽 위(사선방향), 예를 들어 y + 10f
        transform.position = new Vector3(targetPos.x + 5f, targetPos.y + 5f, targetPos.z);
    }

    void Update()
    {
        // 아래로 낙하
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, fallSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // 타격 처리 (범위 공격 가능)
            HitTarget();
        }
    }

    void HitTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.5f, LayerMask.GetMask("Monster"));
        foreach (var col in hits)
        {
            if (col.TryGetComponent(out BaseMonster baseMonster))
                baseMonster.TakeDamage(damage);
            else if (col.TryGetComponent(out BossBaseMonster boss))
                boss.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
