using UnityEngine;

public class Melee : BaseMonster
{
    public float attackRange = 0.5f; // 공격 범위

    // 공격 판정만 처리 (애니메이션은 BaseMonster에서 자동 실행됨)
    protected override void Attack()
    {
        // 공격 범위 내에 Weapon 레이어(무기)가 있는지 확인
        Collider2D weaponCollider = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Weapon"));

        if (weaponCollider != null)
        {
            // 무기 오브젝트에서 WeaponManager 컴포넌트 찾기
            WeaponManager weaponManager = weaponCollider.GetComponent<WeaponManager>();
            if (weaponManager != null)
            {
                weaponManager.TakeWeaponLifeDamage(); // 무기 내구도 감소
                Debug.Log("근접 공격으로 무기 내구도 감소!");
            }
        }
    }

    // 디버그용 공격 범위 표시
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
