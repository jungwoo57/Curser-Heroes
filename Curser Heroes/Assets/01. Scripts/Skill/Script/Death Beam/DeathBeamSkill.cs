using System.Collections;
using UnityEngine;

public class DeathBeamSkill : MonoBehaviour
{
    private SkillManager.SkillInstance skillInstance;

    [Header("Death Beam Settings")]
    [SerializeField] private float procChance = 0.05f;  // 발동 확률 (5%)
    [SerializeField] private int hitCount = 5;          // 총 타격 횟수
    [SerializeField] private float tickInterval = 0.05f;
    [SerializeField] private float targetSearchRadius = 20f;

    [Header("Beam Prefab Parts")]
    [SerializeField] private Transform beamPivot;         // 회전 적용 대상 (기준 오브젝트)
    [SerializeField] private GameObject beamSprite;       // 회전에 영향을 받지 않는 애니메이션 파트
    [SerializeField] private GameObject beamColliderObj;  // 실제 충돌 처리 파트 (자식에 BoxCollider2D 포함)

    private Coroutine beamRoutine;
    public GameObject deathBeamPrefab;
    public CursorWeapon cursorWeapon;

    public void Init(SkillManager.SkillInstance instance)
    {
        skillInstance = instance;
        StartCoroutine(ActivateBeam());
    }

    private IEnumerator ActivateBeam()
    {
        // 가장 가까운 대상 찾기
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, targetSearchRadius, LayerMask.GetMask("Monster"));
        if (hits.Length == 0)
        {
            Destroy(gameObject);
            yield break;
        }

        Transform target = GetClosestTarget(hits);
        if (target == null)
        {
            Destroy(gameObject);
            yield break;
        }

        // 방향 계산 및 회전 적용
        Vector2 direction = (target.position - cursorWeapon.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 프리팹 생성 위치
        GameObject obj = Instantiate(deathBeamPrefab, cursorWeapon.transform.position, Quaternion.identity);

        // 회전 적용
        obj.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Damage Coroutine 실행
        beamRoutine = StartCoroutine(DamageRoutine());

        // 일정 시간 후 삭제
        yield return new WaitForSeconds(hitCount * tickInterval + 0.1f);
        Destroy(gameObject);
    }

    private IEnumerator DamageRoutine()
    {
        int currentHits = 0;

        while (currentHits < hitCount)
        {
            Collider2D[] targets = Physics2D.OverlapBoxAll(
                beamColliderObj.transform.position,
                beamColliderObj.GetComponent<BoxCollider2D>().size,
                beamPivot.eulerAngles.z,
                LayerMask.GetMask("Monster")
            );

            foreach (var target in targets)
            {
                var health = target.GetComponent<BaseMonster>();
                if (health != null)
                {
                    health.TakeDamage(skillInstance.GetCurrentLevelData().damage);
                }
            }

            currentHits++;
            yield return new WaitForSeconds(tickInterval);
        }
    }

    private Transform GetClosestTarget(Collider2D[] hits)
    {
        float minDist = float.MaxValue;
        Transform closest = null;

        foreach (var hit in hits)
        {
            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = hit.transform;
            }
        }

        return closest;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, targetSearchRadius);
    }
}