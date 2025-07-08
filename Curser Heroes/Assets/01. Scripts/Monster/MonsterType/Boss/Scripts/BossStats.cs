// BossStats.cs
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

// 보스의 체력, 피격, 사망 처리를 담당하는 스크립트
public class BossStats : MonoBehaviour
{
    public BossData data;           // ScriptableObject 참조
   private int currentHP;          // 현재 체력

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    BossPatternController BossPatternController; // 보스 패턴 컨트롤러 참조

    private void Awake()
    {
        // 초기 체력 세팅
        currentHP = data.maxHP;
        // 애니메이터와 스프라이트 렌더러 가져오기
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    // 외부에서 호출하여 데미지를 입힐 때 쓰는 함수
    public void TakeDamage(int amount)
    {
        
        if (currentHP <= 0) return;  // 이미 죽었으면 무시

        currentHP -= amount;
        animator.SetTrigger("BossDamage");  // 피격 애니메이션
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
        animator.SetTrigger("BossRun");  // 여기서는 보스 사망 애니메이션으로 사용

        WaveManager.Instance?.OnMonsterKilled(this.gameObject);
        Destroy(gameObject, 5f);         // 5초 뒤 오브젝트 제거
    }
}
