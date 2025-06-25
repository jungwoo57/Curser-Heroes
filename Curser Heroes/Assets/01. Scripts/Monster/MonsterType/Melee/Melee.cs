using UnityEngine;

public class Melee : BaseMonster
{
    public float attackRange = 0.5f; // 공격 범위

    // 공격 실행 함수 (BaseMonster의 추상 메서드 구현)
    protected override void Attack()
    {
        // 공격 범위 내에 Weapon 레이어(무기)를 가진 오브젝트 있는지 확인
        Collider2D weaponCollider = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Weapon"));

        if (weaponCollider != null)
        {
            // 무기 오브젝트에서 WeaponManager 컴포넌트 찾기
            WeaponManager weaponManager = weaponCollider.GetComponent<WeaponManager>();
            if (weaponManager != null)
            {
                // 무기 내구도 감소시키기
                weaponManager.TakeWeaponLifeDamage();
                Debug.Log("근접 공격으로 무기 내구도 감소!");
            }
        }
    }

    // 씬에서 공격 범위 시각화 (디버그용)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
