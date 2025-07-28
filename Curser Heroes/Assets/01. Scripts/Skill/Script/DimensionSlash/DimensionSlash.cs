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

    public void Init(SkillManager.SkillInstance instance)
    {
        skillInstance = instance;
        damage = skillInstance.GetCurrentLevelData().damage;

        damagedTargets = new HashSet<BaseMonster>();
  
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            return;
        }
        animator.Play("Ready");
        StartCoroutine(HandleSlashLifecycle());
    }

    private IEnumerator HandleSlashLifecycle()
    {
        animator.Play("Ready");

        // Ready 애니메이션으로 진입할 때까지 대기
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Ready"));
        // Ready 애니메이션 길이만큼 대기
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Attack 애니메이션 시작
        animator.Play("Attack");

        // 실제로 Attack 애니메이션 상태에 들어갈 때까지 기다림
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"));

        // 이 시점부터 isAttacking 활성화
        isAttacking = true;
        Debug.Log("[DimensionSlash] isAttacking = true, 공격 시작");

        // 공격 유지 시간
        yield return new WaitForSeconds(attackDuration);

        isAttacking = false;
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log($"[TEST] 충돌 발생: {collision.name}, isAttacking: {isAttacking}");


        if (!isAttacking) return;

        // 오직 monsterLayer에 속한 오브젝트만 처리
        if ((monsterLayer.value & (1 << collision.gameObject.layer)) == 0) return;

        BaseMonster monster = collision.GetComponent<BaseMonster>();
        if (monster == null)
        {
            Debug.LogWarning($"[DimensionSlash] {collision.name} 에 BaseMonster 컴포넌트가 없습니다.");
        }
        else if (!damagedTargets.Contains(monster))
        {
            Debug.Log($"DimensionSlash 데미지 적용: {damage} to {monster.name}");
            monster.TakeDamage(damage);
            damagedTargets.Add(monster);
        }

        BossStats boss = collision.GetComponent<BossStats>();
        if (boss != null)
        {
            boss.TakeDamage(damage);
        }
    }
}