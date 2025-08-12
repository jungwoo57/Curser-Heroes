using System.Collections;
using UnityEngine;

public class DeathBeamSkill : MonoBehaviour
{
    private SkillManager.SkillInstance skillInstance;

    [Header("Death Beam Settings")]
    [SerializeField] private float procChance = 0.05f;
    [SerializeField] private int hitCount = 5;
    [SerializeField] private float tickInterval = 0.05f;
    [SerializeField] private float targetSearchRadius = 10f;

    [Header("Beam Prefab Parts")]
    [SerializeField] private Transform beamPivot;         // 회전 대상
    [SerializeField] private GameObject beamSprite;       // 애니메이션 전용 (회전 제외)
    [SerializeField] private GameObject beamColliderObj;  // 충돌 감지 (BoxCollider2D 포함)

    private CursorWeapon cursorWeapon;
    private Coroutine beamRoutine;

    private AudioSource audioSource;

    public void Init(SkillManager.SkillInstance instance)
    {
        skillInstance = instance;

        cursorWeapon = WeaponManager.Instance.cursorWeapon;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (cursorWeapon == null)
        {
            Debug.LogWarning("CursorWeapon이 존재하지 않습니다.");
            Destroy(gameObject);
            return;
        }

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

        // 커서 기준 방향 계산
        Vector2 direction = (target.position - cursorWeapon.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // DeathBeamSkill 위치 및 회전 설정
        transform.position = cursorWeapon.transform.position;

        // 회전 적용
        beamPivot.rotation = Quaternion.Euler(0, 0, angle);

        yield return new WaitForSeconds(1f);
        // 데미지 처리 시작
        beamRoutine = StartCoroutine(DamageRoutine());

        if (skillInstance.skill.audioClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(skillInstance.skill.audioClip);
        }

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
                Vector2.Scale(beamColliderObj.GetComponent<BoxCollider2D>().size, beamColliderObj.transform.lossyScale),
                beamPivot.eulerAngles.z,
                LayerMask.GetMask("Monster")
            );

            Debug.Log($"[{currentHits}] 충돌한 몬스터 수: {targets.Length}");

            foreach (var target in targets)
            {
                BaseMonster monster = target.GetComponentInParent<BaseMonster>();
                if (monster != null)
                {
                    Debug.Log($"[{currentHits}] {monster.name}에게 {skillInstance.GetCurrentLevelData().damage} 데미지");
                    monster.TakeDamage(skillInstance.GetCurrentLevelData().damage);
                    continue;  // 처리했으면 보스 체크 안함
                }

                // 보스 몬스터 처리
                BossStats boss = target.GetComponentInParent<BossStats>();
                if (boss != null)
                {
                    Debug.Log($"[{currentHits}] 보스 {boss.name}에게 {skillInstance.GetCurrentLevelData().damage} 데미지");
                    boss.TakeDamage(skillInstance.GetCurrentLevelData().damage);
                }
                else
                {
                    Debug.LogWarning("충돌했지만 BaseMonster 또는 BossStats 없음: " + target.name);
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