using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meerpokkat : BaseMonster
{
    public float attackRange = 0.5f;
    public float explodeRange = 0.8f;
    protected override void Attack()
    {
        StartCoroutine(AttackReadyTime());
    }

    
    [ContextMenu("자폭공격")]
    protected override void Die()
    {
        if (isDead) return;
        base.Die();
        StartCoroutine(SelfDestruct());
    }

    IEnumerator AttackReadyTime() // 1.0초후에 공격
    {
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(AttackReadyTime());
        Collider2D weaponCollider = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Weapon"));
        if (weaponCollider != null)
        {
            if (WeaponManager.Instance != null)
            {
                WeaponManager.Instance.TakeWeaponLifeDamage();
                Debug.Log("근접 공격");
            }
            else
            {
                Debug.LogWarning("WeaponManager 인스턴스를 찾을 수 없습니다!");
            }
        }
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(1.0f);
        Debug.Log("자폭공격실행됨");
        Collider2D weaponCollider = Physics2D.OverlapCircle(transform.position, explodeRange, LayerMask.GetMask("Weapon"));
        if (weaponCollider != null)
        {
            if (WeaponManager.Instance != null)
            {
                WeaponManager.Instance.TakeWeaponLifeDamage();
                Debug.Log("폭탄 공격");
            }
            else
            {
                Debug.LogWarning("WeaponManager 인스턴스를 찾을 수 없습니다!");
            }
        }
    }
}
