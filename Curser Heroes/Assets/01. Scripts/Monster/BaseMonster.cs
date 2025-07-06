﻿using UnityEngine;
using System;
using System.Collections;

public abstract class BaseMonster : MonoBehaviour
{
    protected int maxHP;
    protected int currentHP;
    protected int damage;
    protected float attackCooldown;
    protected float attackTimer;
    

    protected int valueCost;
    protected Animator animator;
    

    private static readonly int HashAtk = Animator.StringToHash("Atk");
    private static readonly int HashDie = Animator.StringToHash("Die");
    private static readonly int HashDamage = Animator.StringToHash("Damage");
    private static readonly int HashSpawn = Animator.StringToHash("Spw");

    public event Action<GameObject> onDeath;

    private SpriteRenderer spriteRenderer;
    private Coroutine flashCoroutine;
    private Coroutine attackColorCoroutine;
    private Animator effectAnimator;
    protected EffectManager effectManager;  
    public bool IsDead => currentHP <= 0;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogWarning($"{gameObject.name}에 Animator 컴포넌트가 없습니다!");

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            Debug.LogWarning($"{gameObject.name}에 SpriteRenderer 컴포넌트가 없습니다!");
        
      

        PlaySpawnAnimation();
        effectManager = GetComponent<EffectManager>();
    }

    protected virtual void PlaySpawnAnimation()
    {
        if (animator != null)
            animator.SetBool(HashSpawn, true);
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

        // 공격 준비 시 색상 천천히 연한 빨강(분홍)으로 변화
        if (attackTimer <= 1.2f && attackTimer + Time.deltaTime > 1.2f)
        {
            SetAttackBool(true);

            if (spriteRenderer != null)
            {
                if (attackColorCoroutine != null)
                    StopCoroutine(attackColorCoroutine);

                Color softRed = new Color(1f, 0.5f, 0.7f); // 연한 빨강(분홍)
                attackColorCoroutine = StartCoroutine(ChangeColorGradually(softRed, 1.2f));
            }
        }

        // 공격 실행 시 원래 색으로 복구
        if (attackTimer <= 0f)
        {
            Attack();
            SetAttackBool(false);

            if (spriteRenderer != null)
            {
                if (attackColorCoroutine != null)
                    StopCoroutine(attackColorCoroutine);

                attackColorCoroutine = StartCoroutine(ChangeColorGradually(Color.white, 0.3f));
            }

           

            attackTimer = attackCooldown;
        }
    }

    protected void SetAttackBool(bool value)
    {
        if (animator != null)
            animator.SetBool(HashAtk, value);
    }

    public virtual void TakeDamage(int amount, SubWeaponData weaponData = null)
    {
        currentHP -= amount;

        // 이펙트 적용
        if (weaponData != null && effectManager != null)
        {
            IEffect effect = EffectFactory.CreateEffect(weaponData.effect);
            if (effect != null)
            {
                effectManager.AddEffect(effect);
            }
        }


        if (animator != null)
        {
            animator.SetBool(HashDamage, true);
            StopAllCoroutines();
            StartCoroutine(ResetDamageBool());
        }

        // 피격 시 알파 깜빡임 효과 실행
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(FlashAlpha(0.5f));

        if (currentHP <= 0)
            Die();
        //else
        //    PlayHitEffect();     
    }

    private IEnumerator ChangeColorGradually(Color targetColor, float duration)
    {
        if (spriteRenderer == null) yield break;

        float time = 0f;
        Color startColor = spriteRenderer.color;

        while (time < duration)
        {
            spriteRenderer.color = Color.Lerp(startColor, targetColor, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = targetColor;
    }

    private IEnumerator FlashAlpha(float duration)
    {
        if (spriteRenderer == null) yield break;

        float time = 0f;
        Color originalColor = spriteRenderer.color;

        while (time < duration)
        {
            float alpha = Mathf.PingPong(time * 8f, 1f);
            Color c = originalColor;
            c.a = alpha;
            spriteRenderer.color = c;

            time += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = originalColor;
    }

    private IEnumerator ResetDamageBool()
    {
        yield return new WaitForSeconds(0.3f);
        if (animator != null)
            animator.SetBool(HashDamage, false);
    }
  

    public virtual void Stun()     //이펙트 추가
    {
     
        Debug.Log($"{gameObject.name} 몬스터 기절!");
    }

    public virtual void UnStun()
    {
        Debug.Log($"{gameObject.name} 기절 해제됨");
    }

    protected virtual void Die()
    {
        if (animator != null)
            animator.SetBool(HashDie, true);

        onDeath?.Invoke(gameObject);
        Destroy(gameObject, 1f);
    }

    protected abstract void Attack();
}
