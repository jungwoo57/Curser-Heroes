using System.Collections;
using UnityEngine;

public class DeathBeamSkill : MonoBehaviour
{
    private SkillManager.SkillInstance skillInstance;

    [Header("Death Beam Settings")]
    [SerializeField] private float procChance = 0.9f;       // 발동 확률
    [SerializeField] private int hitCount = 5;                // 틱 수
    [SerializeField] private float tickInterval = 0.05f;      // 틱 간격
    [SerializeField] private float targetSearchRadius = 20f;

    [Header("Effects")]
    [SerializeField] private LayerMask monsterLayer;
    [SerializeField] private Animator beamAnimator;

    public void Init(SkillManager.SkillInstance instance)
    {
        this.skillInstance = instance;
    }

    public void TryActivate()
    {
        if (Random.value > procChance)
        {
            Destroy(gameObject); // 발동 실패
            return;
        }

        Transform target = FindNearestMonster();
        if (target == null)
        {
            Destroy(gameObject); // 대상 없음
            return;
        }

        // 방향 조정
        Vector3 dir = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        StartCoroutine(FireBeam(target));
    }

    private IEnumerator FireBeam(Transform target)
    {
        // 애니메이션 트리거 실행
        if (beamAnimator != null)
        {
            beamAnimator.SetTrigger("Fire");
        }

        // 애니메이션 길이만큼 대기 (실제 길이에 맞게 조정)
        yield return new WaitForSeconds(1f);

        int damage = skillInstance.GetCurrentLevelData().damage;

        for (int i = 0; i < hitCount; i++)
        {
            if (target == null) break;

            var monster = target.GetComponent<BaseMonster>();
            if (monster != null)
                monster.TakeDamage(damage);

            yield return new WaitForSeconds(tickInterval);
        }

        Destroy(gameObject);
    }

    private Transform FindNearestMonster()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, targetSearchRadius, monsterLayer);
        Transform nearest = null;
        float minDist = float.MaxValue;

        foreach (var hit in hits)
        {
            float dist = Vector2.Distance(hit.transform.position, transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = hit.transform;
            }
        }

        return nearest;
    }
}