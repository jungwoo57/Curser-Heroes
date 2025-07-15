using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFollowWeapon : MonoBehaviour
{
    [Header("무기 레이어 마스크")]
    public LayerMask weaponLayerMask;   

    [Header("무기 탐지 반경")]
    public float detectionRadius = 10f;

    [Header("추적 속도")]
    public float speed = 1f;

    private Rigidbody2D rb;
    private Transform targetWeapon;
    private BossPatternController patternController;
    private Animator animator;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        patternController = GetComponent<BossPatternController>();
        animator = GetComponentInChildren<Animator>();

    }

    private void FixedUpdate()
    {
        if (patternController.IsInPattern)
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("BossMove", false);
            return;
            
        }
      
        if (patternController.IsInSpawn)
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("BossMove", false);
            return;
            
        }
        FindWeaponByLayer();
        if (patternController.IsDead)
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("BossMove", false);
            return;
        }
        animator.SetBool("BossMove", true);
        if (targetWeapon != null)
        {
            Vector2 direction = (targetWeapon.position - transform.position).normalized;
            rb.velocity = direction * speed;
           
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void FindWeaponByLayer()
    {
        // 지정한 반경 내에서 Weapon 레이어에 속한 Collider2D 모두 탐색
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius, weaponLayerMask);

        if (hits.Length == 0)
        {
            targetWeapon = null;
            return;
        }

        // 그중 가장 가까운 무기 선택
        float minDist = float.MaxValue;
        Transform closest = null;

        foreach (var col in hits)
        {
            float dist = Vector2.Distance(transform.position, col.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = col.transform;
            }
        }
       
        targetWeapon = closest;
    }

}
