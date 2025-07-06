using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
public class BossFollowWeapon : MonoBehaviour
{
    public float speed = 5f; // 무기 따라가는 속도

    private Rigidbody2D rb;
    private Transform targetWeapon;
    public float detectionRadius = 10f;

    private void Awake()
    {
        // Rigidbody2D 컴포넌트 가져와서 회전 고정
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }
    void Update()
    {
        FindWeapon();
       if (targetWeapon == null)
        {
            return;
        }
       if(targetWeapon != null)
        {
            // 무기를 향해 이동
            Vector2 direction = (targetWeapon.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
        else
        {
            rb.velocity = Vector2.zero; // 무기가 없으면 정지
        }
    }
    private void FindWeapon()
    {
        // "Weapon" 태그를 가진 오브젝트 찾기
        GameObject weaponObject = GameObject.FindGameObjectWithTag("Weapon");
        if (weaponObject != null)
        {
            targetWeapon = weaponObject.transform; // 무기의 Transform 저장
        }
        else
        {
            targetWeapon = null; // 무기가 없으면 null로 설정
        }
    }
}
