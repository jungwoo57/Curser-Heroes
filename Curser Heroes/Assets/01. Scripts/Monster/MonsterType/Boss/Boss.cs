using UnityEngine;
using System;

public class Boss : MonoBehaviour
{
    [Header("보스 기본 설정")]
    public int maxHP = 3500;
    private int currentHP;
    private bool isDead = false;

    [Header("패턴 범위 및 딜레이")]
    public float pattern1Range = 2.5f;
    public float pattern2Range = 3.0f;
    public float pattern3Range = 1.5f;

    public float pattern1Delay = 6f;
    public float pattern2Delay = 19f;
    public float pattern3Delay = 5f;

    [Header("이동 설정")]
    public float moveSpeed = 1.5f;
    public GameObject cursorTarget;

    [Header("패턴 이펙트")]
    public GameObject jumpWarningPrefab;

    private enum BossState { Idle, Pattern1, Pattern2, Pattern3, Dead }
    private BossState currentState = BossState.Idle;

    private Animator animator;
    private float patternTimer = 0f;
    private bool isPatternActive = false;
    private float currentPatternDelay = 0f;

    public event Action<GameObject> onDeath;

    private void Start()
    {
        currentHP = maxHP;
        animator = GetComponent<Animator>();
        FindCursorIfMissing();

        ResetAllPatternBools();
        patternTimer = UnityEngine.Random.Range(2f, 4f);
    }

    private void Update()
    {
        if (isDead) return;
        if (cursorTarget == null) return;

        if (currentState == BossState.Idle)
        {
            MoveToCursor();
            patternTimer -= Time.deltaTime;
            if (patternTimer <= 0f)
                StartRandomPattern();
        }

        if (isPatternActive)
        {
            currentPatternDelay -= Time.deltaTime;
            if (currentPatternDelay <= 0f)
                EndPattern();
        }
    }

    private void StartRandomPattern()
    {
        int rnd = UnityEngine.Random.Range(1, 4);
        currentState = (BossState)rnd;
        isPatternActive = true;
        ResetAllPatternBools();

        switch (currentState)
        {
            case BossState.Pattern1:
                animator.SetBool("Pattern1", true);
                TryHitPattern(pattern1Range, 1);
                currentPatternDelay = pattern1Delay;
                break;

            case BossState.Pattern2:
                animator.SetBool("Pattern2", true);
                if (jumpWarningPrefab)
                    Instantiate(jumpWarningPrefab, cursorTarget.transform.position, Quaternion.identity);
                TryHitPattern(pattern2Range, 2);
                currentPatternDelay = pattern2Delay;
                break;

            case BossState.Pattern3:
                animator.SetBool("Pattern3", true);
                TryHitPattern(pattern3Range, 1);
                currentPatternDelay = pattern3Delay;
                break;
        }
    }

    private void EndPattern()
    {
        ResetAllPatternBools();
        currentState = BossState.Idle;
        isPatternActive = false;
        patternTimer = UnityEngine.Random.Range(2f, 4f);
    }

    private void TryHitPattern(float range, int hitCount)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range, LayerMask.GetMask("Weapon"));
        foreach (var hit in hits)
        {
            if (hit != null && WeaponManager.Instance != null)
            {
                for (int i = 0; i < hitCount; i++)
                    WeaponManager.Instance.TakeWeaponLifeDamage();
                break;
            }
        }
    }

    private void MoveToCursor()
    {
        Vector2 dir = (cursorTarget.transform.position - transform.position);
        if (dir.magnitude > 0.1f)
        {
            transform.position += (Vector3)(dir.normalized * moveSpeed * Time.deltaTime);
            animator?.SetBool("IsMoving", true);

            Vector3 scale = transform.localScale;
            scale.x = dir.x > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else
        {
            animator?.SetBool("IsMoving", false);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHP -= damage;
        animator?.SetBool("Damage", true);
        Invoke(nameof(ResetDamage), 0.3f);

        if (currentHP <= 0)
            Die();
    }

    private void ResetDamage()
    {
        animator?.SetBool("Damage", false);
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        animator?.SetBool("Die", true);
        currentState = BossState.Dead;

        onDeath?.Invoke(gameObject);
        Destroy(gameObject, 2f);
    }

    private void ResetAllPatternBools()
    {
        animator?.SetBool("Pattern1", false);
        animator?.SetBool("Pattern2", false);
        animator?.SetBool("Pattern3", false);
    }

    private void FindCursorIfMissing()
    {
        if (cursorTarget != null) return;

        GameObject[] all = FindObjectsOfType<GameObject>();
        foreach (var obj in all)
        {
            if (obj.layer == LayerMask.NameToLayer("Weapon"))
            {
                cursorTarget = obj;
                break;
            }
        }
    }
}
