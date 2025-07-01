// BossBaseMonster.cs
using UnityEngine;
using System;
using System.Collections;

public abstract class BossBaseMonster : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private MonsterData monsterData;
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1.5f;

    [Header("Pattern Settings")]
    [SerializeField] private float initialDelay = 3f;
    [SerializeField] private float patternCooldown = 5f;
    [SerializeField] private float preFlashDuration = 0.5f;

    protected int maxHP;
    protected int currentHP;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected Color originalColor;
    protected Transform target;

    private bool canMove = true;
    private int weaponLayerMask;
    private Coroutine flashCoroutine;

    private static readonly int HashDie = Animator.StringToHash("Die");
    private static readonly int HashDamage = Animator.StringToHash("Damage");
    private static readonly int HashIsMoving = Animator.StringToHash("IsMoving");

    public event Action<GameObject> onDeath;

    public virtual void Setup(MonsterData data)
    {
        if (data == null)
        {
            Debug.LogWarning("MonsterData가 설정되지 않았습니다!");
            return;
        }

        maxHP = data.maxHP;
        currentHP = maxHP;
        weaponLayerMask = LayerMask.GetMask("Weapon");
    }

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        Setup(monsterData);
        StartCoroutine(BossPatternLoop());
    }

    protected virtual void Update()
    {
        if (canMove)
        {
            if (target == null || !target.gameObject.activeInHierarchy)
                FindClosestTarget();
            MoveTowardsTarget();
        }
        else
        {
            animator?.SetBool(HashIsMoving, false);
        }
    }

    private void MoveTowardsTarget()
    {
        if (target == null) return;
        Vector3 dir = target.position - transform.position;
        if (dir.sqrMagnitude > 0.01f)
        {
            dir.Normalize();
            transform.position += dir * moveSpeed * Time.deltaTime;
            animator?.SetBool(HashIsMoving, true);
        }
        else
        {
            animator?.SetBool(HashIsMoving, false);
        }
    }

    private IEnumerator BossPatternLoop()
    {
        // 1) 초기 대기
        yield return new WaitForSeconds(initialDelay);

        while (currentHP > 0)
        {
            // 2) 패턴 전 5초 쿨다운 (이동 가능)
            canMove = true;
            FindClosestTarget();
            yield return new WaitForSeconds(patternCooldown);

            // 3) 예고 플래시 & 정지
            canMove = false;
            animator?.SetBool(HashIsMoving, false);
            yield return PreAttackFlash(preFlashDuration);

            // 4) 패턴 실행
            int p = UnityEngine.Random.Range(1, 4);
            yield return ExecutePattern(p);
        }
    }

    private IEnumerator ExecutePattern(int p)
    {
        switch (p)
        {
            case 1: yield return Pattern1(); break;
            case 2: yield return Pattern2(); break;
            case 3: yield return Pattern3(); break;
        }
    }

    public virtual void TakeDamage(int amt)
    {
        currentHP -= amt;
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
        float t = 0f;
        while (t < duration)
        {
            float a = Mathf.PingPong(t * 8f, 1f);
            var c = originalColor; c.a = a;
            spriteRenderer.color = c;
            t += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = originalColor;
    }

    private IEnumerator ResetDamageBool()
    {
        yield return new WaitForSeconds(0.3f);
        animator?.SetBool(HashDamage, false);
    }

    protected virtual void Die()
    {
        animator?.SetBool(HashDie, true);
        onDeath?.Invoke(gameObject);
        Destroy(gameObject, 2f);
    }

    private IEnumerator PreAttackFlash(float duration)
    {
        if (spriteRenderer == null) yield break;
        float t = 0f;
        while (t < duration)
        {
            float lerp = Mathf.PingPong(t * 2f, 1f);
            spriteRenderer.color = Color.Lerp(originalColor, Color.magenta, lerp);
            t += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = originalColor;
    }

    protected void FindClosestTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 100f, weaponLayerMask);
        float minDist = Mathf.Infinity;
        Transform best = null;
        foreach (var c in hits)
        {
            if (c == null || !c.gameObject.activeInHierarchy) continue;
            float d = (c.transform.position - transform.position).sqrMagnitude;
            if (d < minDist)
            {
                minDist = d;
                best = c.transform;
            }
        }
        target = best;
    }

    protected abstract IEnumerator Pattern1();
    protected abstract IEnumerator Pattern2();
    protected abstract IEnumerator Pattern3();
}
