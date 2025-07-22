using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeerKat : BaseMonster
{
    public float attackRange = 0.5f;
    protected override void Attack()
    {
        StartCoroutine(AttackReadyTime());
    }

    IEnumerator AttackReadyTime() // 1초후에 공격
    {
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(AttackReadyTime());
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
}
