using System;
using System.Collections;
using UnityEngine;

public class BigGreenSlime : BaseMonster
{
    public float attackRange = 0.5f;

    protected override void Attack()
    {
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

        StartCoroutine(SelfDestructAfterDelay(0.1f));
        Die();
    }

    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    private IEnumerator SelfDestructAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
}
