using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Weapon"))
            return;
        
       WeaponLife weapon = other.GetComponent<WeaponLife>();
        if (weapon != null)
            weapon.TakeLifeDamage(); 

    }
}
