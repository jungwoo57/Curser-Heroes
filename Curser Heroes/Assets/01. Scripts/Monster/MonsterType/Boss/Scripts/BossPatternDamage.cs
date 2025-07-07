using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
public class BossPatternDamage : MonoBehaviour
{
    private Collider2D col;
    public float cooldownTime = 1; // 공격 쿨타임 (초 단위)


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
            if (cooldownTime <= 0)
            {
                Attack(); // 콜라이더가 활성화 되어 있으면 공격 함수 호출
            }
                      
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
    public void cooltime()
    {
        if (cooldownTime > 0)
        {
            cooldownTime -= Time.deltaTime; // 쿨타임 감소
            if (cooldownTime <= 0)
            {
                cooldownTime = 1; // 쿨타임이 0 이하로 내려가면 0으로 설정
            }
        }
    }
    
}
