using System.Collections;
using UnityEngine;

public class DeathBeamSkill : MonoBehaviour
{
    private SkillManager.SkillInstance skillInstance;

    [Header("Death Beam Settings")]
    [SerializeField] private float procChance = 1f;
    [SerializeField] private int hitCount = 5;
    [SerializeField] private float tickInterval = 0.05f;
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
        Debug.Log("[DeathBeamSkill] TryActivate 호출됨");

        if (Random.value > procChance)
        {
            Debug.Log("[DeathBeamSkill] 확률 실패로 종료");
            Destroy(gameObject);
            return;
        }

        Transform target = FindNearestMonster();
        if (target == null)
        {
            Debug.Log("[DeathBeamSkill] 타겟 없음 → 종료");
            Destroy(gameObject);
            return;
        }

        // 타겟 방향 회전
        Vector3 dir = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Debug.Log($"[DeathBeamSkill] 타겟 방향 회전 완료, angle: {angle}");
        StartCoroutine(FireBeam(target));
    }

    private IEnumerator FireBeam(Transform target)
    {
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