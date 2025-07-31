using System.Collections;
using UnityEngine;

public class Sandworm : BaseMonster
{
    public float attackDelay = 1f;
    public float detectionRadius = 10f;

    public GameObject effectPrefab;                 // 침 이펙트 프리팹
    public WarningIndicator warningIndicator;       // 인스펙터에서 직접 할당 (비활성 상태)

    protected override void Attack()
    {
        Transform target = FindNearestWeapon();
        if (target == null) return;

        // 1. 고정 위치 저장
        Vector3 fixedPos = target.position;

        // 2. 가짜 타겟 생성 (1회용)
        GameObject fakeTarget = new GameObject("WarningTarget");
        fakeTarget.transform.position = fixedPos;

        // 3. 경고 설정
        warningIndicator.target = fakeTarget.transform;
        warningIndicator.duration = attackDelay;
        warningIndicator.gameObject.SetActive(true);

        // 4. 공격 후 fakeTarget 제거
        StartCoroutine(SpawnRootAfterDelay(fixedPos, fakeTarget));
    }



    private Transform FindNearestWeapon()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius, LayerMask.GetMask("Weapon"));
        float minDist = float.MaxValue;
        Transform nearest = null;

        foreach (var hit in hits)
        {
            float dist = (hit.transform.position - transform.position).sqrMagnitude;
            if (dist < minDist)
            {
                minDist = dist;
                nearest = hit.transform;
            }
        }

        return nearest;
    }

    IEnumerator SpawnRootAfterDelay(Vector3 pos, GameObject fakeTarget)
    {
        yield return new WaitForSeconds(attackDelay);
        
        GameObject acidPrefab = Instantiate(effectPrefab, pos, Quaternion.identity ,transform);
        Octopus_Ink acidEffect = acidPrefab.GetComponent<Octopus_Ink>();
        acidEffect.Initialize(Vector3.zero, damage);
        Destroy(acidEffect, 0.5f);
        Destroy(fakeTarget); // 타겟 오브젝트 삭제
    }

}
