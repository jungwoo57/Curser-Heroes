using UnityEngine;
using System;
using System.Collections;

public abstract class BossBaseMonster : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private MonsterData monsterData;

    [Header("Pattern Settings")]
    [SerializeField] private float initialDelay = 3f;
    [SerializeField] private float patternCooldown = 5f;

    protected int maxHP;
    protected int currentHP;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    private Color originalColor;

    private int weaponLayerMask;

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
        // Movement disabled: always idle
        animator?.SetBool(HashIsMoving, false);
    }

    private IEnumerator BossPatternLoop()
    {
        yield return new WaitForSeconds(initialDelay);

        while (currentHP > 0)
        {
            yield return new WaitForSeconds(patternCooldown);

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
        AudioManager.Instance.PlayHitSound(HitType.Monster);
        if (animator != null)
        {
            animator.SetBool(HashDamage, true);
            StartCoroutine(ResetDamageBool());
        }

        if (spriteRenderer != null)
            StartCoroutine(FlashAlpha(0.5f));

        if (currentHP <= 0)
            Die();
    }

    private IEnumerator ResetDamageBool()
    {
        yield return new WaitForSeconds(0.3f);
        animator?.SetBool(HashDamage, false);
    }

    private IEnumerator FlashAlpha(float duration)
    {
        if (spriteRenderer == null) yield break;
        float t = 0f;
        while (t < duration)
        {
            float a = Mathf.PingPong(t * 8f, 1f);
            Color c = originalColor;
            c.a = a;
            spriteRenderer.color = c;
            t += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = originalColor;
    }

    protected virtual void Die()
    {
        animator?.SetBool(HashDie, true);
        onDeath?.Invoke(gameObject);
        Destroy(gameObject, 2f);
    }

    protected abstract IEnumerator Pattern1();
    protected abstract IEnumerator Pattern2();
    protected abstract IEnumerator Pattern3();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Weapon"))
            WeaponManager.Instance?.TakeWeaponLifeDamage();
    }
}
