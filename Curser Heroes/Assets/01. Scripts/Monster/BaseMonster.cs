using UnityEngine;

// 모든 몬스터가 공통으로 상속받는 베이스 클래스.
// 체력, 공격 쿨다운, 피격 처리 등을 포함.

public abstract class BaseMonster : MonoBehaviour
{
    [Header("스탯")]
    public int maxHP;                  // 최대 체력
    public int currentHP;              // 현재 체력
    public int valueCost;              // 밸류 시스템에서의 몬스터 비용
    public float attackCooldown = 2f;  // 공격 쿨다운 시간
    public int damage;              

    protected float attackTimer;       // 쿨다운 타이머

  
    protected virtual void Start()
    {
        currentHP = maxHP;
        attackTimer = Random.Range(0f, attackCooldown); // 첫 공격 시간 랜덤화
    }

    protected virtual void Update()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            Attack();                           // 공격 실행
            attackTimer = attackCooldown;       // 쿨다운 초기화
        }
    }

  
    // 데미지를 받았을 때 호출됨
    
    public virtual void TakeDamage(int amount)
    {
        currentHP -= amount;

        if (currentHP <= 0)
            Die();              // 체력이 0 이하이면 사망 처리
        else
            PlayHitEffect();    // 피격 연출
    }

    
    // 피격 이펙트나 연출 (예: 반짝임)
    
    protected virtual void PlayHitEffect()
    {
        // 피격 시 시각 효과
        // 예: SpriteRenderer 색상 변경 등
    }

   
    // 사망 처리
   
    protected virtual void Die()
    {
        // 사망 시 이펙트, 점수 처리 등 추가 가능
        Destroy(gameObject); // 오브젝트 삭제
    }

  
    // 개별 몬스터마다 공격 로직 다르게 구현
    
    protected abstract void Attack();
}
