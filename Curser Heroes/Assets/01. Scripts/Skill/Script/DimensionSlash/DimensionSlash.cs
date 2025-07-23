using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionSlash : MonoBehaviour
{
    private SkillManager.SkillInstance skillInstance;
    private Animator animator;

    private HashSet<BaseMonster> damagedTargets = new();

    private bool isAttacking = false;

    private int damage;

    [SerializeField] private float attackDuration = 0.2f;
    [SerializeField] private LayerMask monsterLayer;

    public void Init(SkillManager.SkillInstance instance)
    {
        skillInstance = instance;
        damage = skillInstance.GetCurrentLevelData().damage;

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
        // Ready 애니메이션 완료 후 → Attack
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        isAttacking = true;
        animator.Play("Attack");

        yield return new WaitForSeconds(attackDuration);

        isAttacking = false;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isAttacking) return;

        // 몬스터 레이어에 해당하는 오브젝트인지 확인
        if ((monsterLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            BaseMonster monster = collision.GetComponent<BaseMonster>();
            if (monster != null && !damagedTargets.Contains(monster))
            {
                monster.TakeDamage(damage);
                damagedTargets.Add(monster);
            }
        }
    }
}