using UnityEngine;

public class Melee : BaseMonster
{
    [Header("테스트용 데이터 (인스펙터에서 할당 가능)")]
    [SerializeField] private MonsterData monsterData;

    public float attackRange = 0.5f;

    // BaseMonster.Start() 호출 + HP 세팅
    protected override void Start()
    {
        base.Start();

        if (monsterData != null)
        {
            // MonsterData가 있으면 그 데이터로 세팅
            Setup(monsterData);
            Debug.Log($"[Melee] {name} Setup() 호출 – HP={CurrentHP}");
        }
        else
        {
            // 데이터가 없으면 maxHP로 기본 세팅
            if (currentHP <= 0)
            {
                currentHP = maxHP;
                Debug.Log($"[Melee] {name} 기본 HP 세팅 – HP={currentHP}");
            }
        }
    }

    protected override void Attack()
    {
        Collider2D weaponCollider = Physics2D.OverlapCircle(
            transform.position,
            attackRange,
            LayerMask.GetMask("Weapon")
        );

        if (weaponCollider != null)
        {
            if (WeaponManager.Instance != null)
            {
                WeaponManager.Instance.TakeWeaponLifeDamage();
                Debug.Log("근접 공격으로 무기 내구도 감소!");
            }
            else
            {
                Debug.LogWarning("WeaponManager 인스턴스를 찾을 수 없습니다!");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
