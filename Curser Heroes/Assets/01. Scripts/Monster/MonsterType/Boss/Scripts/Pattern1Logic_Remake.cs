using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern1Logic_Remake : PatternLogicBase
{
    [Header("Pattern1 부모 오브젝트 (Pattern 1)")]
    public Transform patternParent;

    [Header("각 자식 활성화 간격 (초)")]
    public float activationInterval = 0.1f;  // 자식 간 켜지는 간격

    [Header("발사 위치 기준(Weapon 방향)")]
    public Transform firePoint;
    public LayerMask weaponLayerMask;
    public float detectionRadius = 10f;
    public Transform targetWeapon;

    [Header("경고 이펙트")]
    public WarningIndicator warningIndicator;
    public float warningTime = 0.6f;

    [Header("지면 공격 관련")]
    [SerializeField] private GameObject ground;
    //[SerializeField] private float ;

    void Start()
    {
        FindWeaponByLayer();
    }
    public override IEnumerator Execute(BossPatternController controller)
    {
        
        if (patternParent == null || firePoint == null || targetWeapon == null)
        {
            Debug.LogWarning("Pattern1Logic: 필수 Transform이 할당되지 않았습니다.");
            yield break;
        }
        Vector2 dir = (targetWeapon.position - patternParent.position).normalized; // 발사 방향
        patternParent.up = -dir; // 발사 방향 조절
        if (warningIndicator != null)
        {
            warningIndicator.transform.position = patternParent.position;  // 위치 그대로
            warningIndicator.transform.rotation = patternParent.rotation;  // 회전만 복사
            warningIndicator.duration = warningTime;
            warningIndicator.gameObject.SetActive(true);
        }
        // 발사 로직 작성
        
        
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
