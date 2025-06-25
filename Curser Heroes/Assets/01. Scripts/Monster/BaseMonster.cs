using UnityEngine;
using System;

public abstract class BaseMonster : MonoBehaviour
{
    // --- 스탯 ---
    protected int maxHP;
    protected int currentHP;
    protected int damage;
    protected float attackCooldown;
    protected float attackTimer;

    public int valueCost;

    // 몬스터 사망 시 호출될 이벤트 (콜백)
    public event Action<GameObject> onDeath;

    // --- 세팅 메서드 ---
    public virtual void Setup(MonsterData data)
    {
        maxHP = data.maxHP;
        currentHP = maxHP;
        damage = data.damage;
        attackCooldown = data.attackCooldown;
        valueCost = data.valueCost;

        attackTimer = attackCooldown; // 시작부터 쿨타임이 돌도록 초기화!
    }

    // --- 매 프레임 호출 ---
    protected virtual void Update()
    {
        if (attackTimer > 0f)
            attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            Attack();
            attackTimer = attackCooldown;
        }
    }

    // --- 데미지 받기 ---
    public virtual void TakeDamage(int amount)
    {
        currentHP -= amount;

        if (currentHP <= 0)
            Die();
        else
            PlayHitEffect();
    }

    // --- 피격 효과 (선택 사항) ---
    protected virtual void PlayHitEffect()
    {
        // 예: 색깔 깜빡임, 이펙트 등 구현 가능
    }

    // --- 사망 처리 ---
    protected virtual void Die()
    {
        onDeath?.Invoke(gameObject);
        Destroy(gameObject);
    }

    // --- 공격 (서브 클래스에서 구현) ---
    protected abstract void Attack();
}
