using UnityEngine;

public class MeteorSkill : MonoBehaviour
{
    private int damage;
    [SerializeField] private float delayBeforeHit = 0.5f; // 충돌 처리 전 대기 시간
    private float timer = 0f;
    //private float fallSpeed = 10f;
    [SerializeField] private Vector3 targetPosition;

    public void Init(int dmg, Vector3 targetPos)
    {
        damage = dmg;
        targetPosition = targetPos;

        // 시작 위치는 targetPosition의 오른쪽 위(사선방향), 예를 들어 y + 10f
        transform.position = new Vector3(targetPos.x, targetPos.y + 0.1f, targetPos.z);
    }

    void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, targetPosition, fallSpeed * Time.deltaTime);
        timer += Time.deltaTime;

        //if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        if (timer >= delayBeforeHit)
        {
            // 타격 처리 (범위 공격 가능)
            HitTarget();
        }
    }

    void HitTarget()
    {
        //Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.5f, LayerMask.GetMask("Monster"));
        Collider2D[] hits = Physics2D.OverlapCircleAll(targetPosition, 0.5f, LayerMask.GetMask("Monster"));
        foreach (var col in hits)
        {
            if (col.TryGetComponent(out BaseMonster baseMonster))
                baseMonster.TakeDamage(damage);
            else if (col.TryGetComponent(out BossStats boss))
                boss.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, 0.5f);
        Gizmos.DrawWireSphere(targetPosition, 0.5f);
    }
}
