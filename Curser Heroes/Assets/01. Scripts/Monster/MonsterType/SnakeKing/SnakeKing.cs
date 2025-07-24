using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeKing : BaseMonster
{
    public float attackRange = 0.5f;
    [SerializeField] private float attackDelay;
    [SerializeField] private float offset;
    protected override void Attack()
    {
        StartCoroutine(DelayAttack());
    }

    IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        Debug.Log("공격");
        Collider2D weaponCollider = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Weapon"));
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
        Gizmos.DrawWireSphere(transform.position + Vector3.up * offset, attackRange);
    }
}
