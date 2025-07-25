using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase : MonoBehaviour
{
    public BossData data;           // ScriptableObject 참조
    public int currentHP;          // 현재 체력
    [SerializeField] private float attackRange;
    //public static event Action<BossBase> OnAnyMonsterDamaged; << ???

    [SerializeField] protected bool isPattern;
    [SerializeField] protected bool isDie;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    protected Vector3 targetPos;
    protected List<System.Action> patterns = new List<System.Action>();
    
    protected virtual void Awake()
    {
        // 초기 체력 세팅
        currentHP = data.maxHP;
        // 애니메이터와 스프라이트 렌더러 가져오기
        //animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    // 외부에서 호출하여 데미지를 입힐 때 쓰는 함수
    public void TakeDamage(int amount)
    {
        
        if (currentHP <= 0) return;  // 이미 죽었으면 무시

        currentHP -= amount;
        //OnAnyMonsterDamaged?.Invoke(this);
        //animator.SetTrigger("BossDamage");  // 피격 애니메이션
        StartCoroutine(FlashEffect());

        if (currentHP <= 0)
            Die();
        
    }
    // 투명도를 깜빡이며 피격 효과
    private IEnumerator FlashEffect()
    {
        float duration = 0.5f;
        float timer = 0f;

        while (timer < duration)
        {
            float alpha = Mathf.PingPong(timer * 4f, 1f);
            var c = originalColor;
            c.a = alpha;
            spriteRenderer.color = c;

            timer += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = originalColor;
    }

    // 죽었을 때 호출
    public void Die()
    {
        //animator.SetTrigger("BossRun"); // 사망 애니메이션
        StartCoroutine(DelayedDeath());
    }

    private IEnumerator DelayedDeath()
    {
        yield return new WaitForSeconds(3.5f); // 연출 대기 시간

        WaveManager.Instance?.OnMonsterKilled(this.gameObject);
        Destroy(gameObject); // 이제 오브젝트 제거
    }

    protected void CheckTarget()
    {
        Collider2D weaponCollider = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Weapon"));
        if (weaponCollider == null) return;
        targetPos = weaponCollider.transform.position;
    }
}
