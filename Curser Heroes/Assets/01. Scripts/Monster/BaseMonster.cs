using UnityEngine;
using System;

public abstract class BaseMonster : MonoBehaviour
{
    protected int maxHP;
    protected int currentHP;
    protected int damage;
    protected float attackCooldown;
    protected float attackTimer;

    protected int valueCost;
    protected Animator animator;

    // bool 파라미터 해시 (이름이 'Atk', 'Die', 'Damage', 'Spw'인 bool 타입)
    private static readonly int HashAtk = Animator.StringToHash("Atk");
    private static readonly int HashDie = Animator.StringToHash("Die");
    private static readonly int HashDamage = Animator.StringToHash("Damage");
    private static readonly int HashSpawn = Animator.StringToHash("Spw");

    public event Action<GameObject> onDeath;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
            Debug.LogWarning($"{gameObject.name}에 Animator 컴포넌트가 없습니다!");

        PlaySpawnAnimation();
    }

   
    protected virtual void PlaySpawnAnimation()
    {
        if (animator != null)
        {
            animator.SetBool(HashSpawn, true);
            //스폰에서 idle로 넘어가는 코드 
           
        }
    }

    public virtual void Setup(MonsterData data)
    {
        maxHP = data.maxHP;
        currentHP = maxHP;
        damage = data.damage;
        attackCooldown = data.attackCooldown;
        valueCost = data.valueCost;

        attackTimer = attackCooldown;
    }

    protected virtual void Update()
    {
        if (attackTimer > 0f)
            attackTimer -= Time.deltaTime;

        if (attackTimer <= 1.2f && attackTimer + Time.deltaTime > 1.2f)
        {
            SetAttackBool(true); // 쿨다운 1.2초 남았을 때 공격 애니메이션 켜기
        }

        if (attackTimer <= 0f)
        {
            Attack();
            SetAttackBool(false); // 공격 후 애니메이션 끄기
            attackTimer = attackCooldown;
        }
    }

    protected void SetAttackBool(bool value)
    {
        if (animator != null)
            animator.SetBool(HashAtk, value);
    }

    public virtual void TakeDamage(int amount)
    {
        currentHP -= amount;

        if (animator != null)
        {
            // 데미지 입을 때 Damage bool true -> false 시퀀스
            animator.SetBool(HashDamage, true);
            // 코루틴으로 잠시 후 false로 변경
            StopAllCoroutines();
            StartCoroutine(ResetDamageBool());
        }

        if (currentHP <= 0)
            Die();
        else
            PlayHitEffect();
    }

    private System.Collections.IEnumerator ResetDamageBool()
    {
        yield return new WaitForSeconds(0.3f); // 0.3초 후 데미지 애니메이션 해제
        if (animator != null)
            animator.SetBool(HashDamage, false);
    }

    protected virtual void PlayHitEffect()
    {
        // 필요시 오버라이드해서 피격 효과 구현
    }

    protected virtual void Die()
    {
        if (animator != null)
            animator.SetBool(HashDie, true);

        onDeath?.Invoke(gameObject);
        Destroy(gameObject, 1f); // 사망 애니메이션 재생 시간 확보
    }

    protected abstract void Attack();
}
