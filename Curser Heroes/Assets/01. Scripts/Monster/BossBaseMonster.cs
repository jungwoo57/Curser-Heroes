using UnityEngine;
using System;
using System.Collections;

public abstract class BossBaseMonster : MonoBehaviour
{
    protected int maxHP;
    protected int currentHP;
    protected float attackCooldown;
    protected float attackTimer;
    protected int damage;
    protected int valueCost;

    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    private Coroutine flashCoroutine;
    private Coroutine patternCoroutine;

    private static readonly int HashDie = Animator.StringToHash("Die");
    private static readonly int HashDamage = Animator.StringToHash("Damage");
    private static readonly int HashPattern1 = Animator.StringToHash("Pattern1");
    private static readonly int HashPattern2 = Animator.StringToHash("Pattern2");
    private static readonly int HashPattern3 = Animator.StringToHash("Pattern3");
    private static readonly int HashIsMoving = Animator.StringToHash("IsMoving");

    public event Action<GameObject> onDeath;

    private Transform target;
    public float moveSpeed = 1.5f;
    private bool isAttacking = false;

    public virtual void Setup(MonsterData data)
    {
        Debug.Log($"[Boss Spawn] {gameObject.name} Setup called: HP = {maxHP}, Damage = {damage}");

        maxHP = data.maxHP;
        currentHP = maxHP;
        attackCooldown = data.attackCooldown;
        damage = data.damage;
        valueCost = data.valueCost;
        attackTimer = attackCooldown;
    }

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameObject targetObj = GameObject.FindGameObjectWithTag("Weapon");
        if (targetObj != null)
            target = targetObj.transform;

        patternCoroutine = StartCoroutine(BossPatternLoop());
    }

    protected virtual void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (target == null || isAttacking)
        {
            if (animator != null) animator.SetBool(HashIsMoving, false);
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        if (animator != null)
            animator.SetBool(HashIsMoving, true);
    }

    protected virtual IEnumerator BossPatternLoop()
    {
        yield return new WaitForSeconds(3f);

        while (currentHP > 0)
        {
            if (!isAttacking)
            {
                int pattern = UnityEngine.Random.Range(1, 4);
                isAttacking = true;

                switch (pattern)
                {
                    case 1:
                        yield return Pattern1();
                        yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 7f));
                        break;
                    case 2:
                        yield return Pattern2();
                        yield return new WaitForSeconds(UnityEngine.Random.Range(18f, 20f));
                        break;
                    case 3:
                        yield return Pattern3();
                        yield return new WaitForSeconds(UnityEngine.Random.Range(4f, 6f));
                        break;
                }

                isAttacking = false;
            }
            yield return null;
        }
    }

    protected virtual IEnumerator Pattern1()
    {
        animator.SetBool(HashPattern1, true);
        yield return new WaitForSeconds(2f);
        animator.SetBool(HashPattern1, false);
    }

    protected virtual IEnumerator Pattern2()
    {
        animator.SetBool(HashPattern2, true);
        yield return new WaitForSeconds(3f);
        animator.SetBool(HashPattern2, false);
    }

    protected virtual IEnumerator Pattern3()
    {
        animator.SetBool(HashPattern3, true);
        yield return new WaitForSeconds(2f);
        animator.SetBool(HashPattern3, false);
    }

    public virtual void TakeDamage(int amount)
    {
        currentHP -= amount;

        if (animator != null)
        {
            animator.SetBool(HashDamage, true);
            StartCoroutine(ResetDamageBool());
        }

        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(FlashAlpha(0.5f));

        if (currentHP <= 0)
            Die();
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

    protected virtual void Die()
    {
        if (animator != null)
            animator.SetBool(HashDie, true);

        onDeath?.Invoke(gameObject);
        Destroy(gameObject, 2f);
    }
}