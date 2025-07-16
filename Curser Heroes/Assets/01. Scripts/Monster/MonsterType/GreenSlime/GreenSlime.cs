using System.Collections;
using UnityEngine;

public class GreenSlime: BaseMonster
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
        StartCoroutine(WaitForAnimationEnd("Attack")); // 애니메이션이 끝날 때까지 대기
       
    }

    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    private IEnumerator WaitForAnimationEnd(string stateName, float timeout = 5f)
    {
        int layer = 0;
        float start = Time.time;
        while (!animator.GetCurrentAnimatorStateInfo(layer).IsName(stateName))
        {
            if (Time.time - start > timeout)
            {
                Destroy(gameObject);
            }
            yield return null;
        }
        while (animator.GetCurrentAnimatorStateInfo(layer).normalizedTime < 1f)
        {
            if (Time.time - start > timeout)
            {

                Destroy(gameObject);
            }
            yield return null;
        }
    }
}
