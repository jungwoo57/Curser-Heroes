using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
public class BossPatternDamage : MonoBehaviour
{
    private Collider2D col;
    

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        col.enabled = false;        // 처음엔 꺼두기
    }
    public void Activate()
    {
        col.enabled = true;  // 패턴이 활성화 되면 콜라이더 활성 
    }
    public void Deactivate() // 패턴이 비활성화 되면 콜라이더 비활성
    {
        col.enabled = false;
    }
    private void Update()
    {
        if (col.enabled)
        {
            Attack(); // 콜라이더가 활성화 되어 있으면 공격 함수 호출
        }
    }

    protected void Attack()
     {
        Collider2D weaponCollider = Physics2D.OverlapCircle(transform.position,  LayerMask.GetMask("Weapon"));
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
