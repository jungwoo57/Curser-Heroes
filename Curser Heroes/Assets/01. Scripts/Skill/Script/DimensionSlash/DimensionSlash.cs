using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionSlash : MonoBehaviour
{
    private SkillManager.SkillInstance skillInstance;
    private Animator animator;
    private HashSet<BaseMonster> damagedTargets = new HashSet<BaseMonster>();

    private bool isAttacking = false;
    private int damage;

    [SerializeField] private float attackDuration = 1f;
    [SerializeField] private LayerMask monsterLayer;

    public void Init(SkillManager.SkillInstance skillInstance)
    {
        this.skillInstance = skillInstance;
        this.damage = skillInstance.skill.levelDataList[skillInstance.level - 1].damage;

        Debug.Log($"[DimensionSlash] Init damage = {damage}");

        animator = GetComponent<Animator>();
        StartCoroutine(HandleSlashLifecycle());
    }

    private IEnumerator HandleSlashLifecycle()
    {
        // Ready 애니메이션 먼저 재생
        animator.Play("Ready");

        // Ready 상태일 때까지 대기
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Ready"));
        // Ready 애니메이션 길이만큼 대기
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // 공격 상태 진입
        isAttacking = true;
        Debug.Log("[DimensionSlash] isAttacking = true, 공격 시작");
        animator.Play("Attack");

        DetectInitialTargets();

        // 공격 애니메이션 길이만큼 대기
        yield return new WaitForSeconds(attackDuration);

        isAttacking = false;
        Debug.Log("[DimensionSlash] isAttacking = false, 공격 종료");

        Destroy(gameObject);
    }
    private void DetectInitialTargets()
    {
        Collider2D[] results = new Collider2D[20]; // 충돌 결과 받을 배열 (크기 충분히 확보)
        ContactFilter2D filter = new ContactFilter2D
        {
            useLayerMask = true,
            layerMask = monsterLayer,
            useTriggers = true
        };

        int hitCount = Physics2D.OverlapCollider(GetComponent<Collider2D>(), filter, results);

        for (int i = 0; i < hitCount; i++)
        {
            HandleCollision(results[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        HandleCollision(collision);
    }

    private void HandleCollision(Collider2D collision)
    {
        Debug.Log($"[DimensionSlash] HandleCollision 호출됨, 충돌 대상: {collision.gameObject.name}, 레이어: {LayerMask.LayerToName(collision.gameObject.layer)}");

        if (!isAttacking) return;

        // 몬스터 레이어 필터링
        if ((monsterLayer.value & (1 << collision.gameObject.layer)) == 0)
        {
            Debug.Log("[DimensionSlash] 충돌 대상이 monsterLayer에 없음. 무시됨.");
            return;
        }

        // 일반 몬스터 처리
        BaseMonster monster = collision.GetComponentInParent<BaseMonster>();
        if (monster != null && !damagedTargets.Contains(monster))
        {
            Debug.Log($"DimensionSlash 데미지 적용: {damage} to {monster.name}");
            monster.TakeDamage(damage);
            damagedTargets.Add(monster);
            return;
        }

        // 보스 처리
        BossStats boss = collision.GetComponentInParent<BossStats>();
        if (boss != null)
        {
            Debug.Log($"DimensionSlash 보스 데미지 적용: {damage} to {boss.name}");
            boss.TakeDamage(damage);
            return;
        }
    }
}