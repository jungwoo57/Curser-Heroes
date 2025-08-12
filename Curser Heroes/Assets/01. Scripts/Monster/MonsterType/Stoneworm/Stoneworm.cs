using System.Collections;
using UnityEngine;

public class Stoneworm : BaseMonster
{
    public float attackDelay = 1f;
    public float detectionRadius = 10f;

    public GameObject effectPrefab;               // 뿌리 이펙트 프리팹
    public WarningIndicator[] warningIndicator; // 프리팹으로 여러 개 인스턴스화 가능하게

    protected override void Attack()
    {
        Transform target = FindNearestWeapon();
        if (target == null) return;

        Vector3 basePos = target.position;

        for (int i = 0; i < warningIndicator.Length; i++)
        {
            // 각 위치에 약간의 오프셋을 줘서 분산
            Vector3 offset = Random.insideUnitCircle * 1.5f;
            Vector3 spawnPos = basePos + offset;

            // 가짜 타겟 생성 (각 경고용)
            GameObject fakeTarget = new GameObject($"WarningTarget_{i}");
            fakeTarget.transform.position = spawnPos;

            /*
            // 경고 인디케이터 인스턴스 생성 및 설정
            WarningIndicator indicator = Instantiate(warningIndicatorPrefab);
            indicator.target = fakeTarget.transform;
            indicator.duration = attackDelay;
            indicator.gameObject.SetActive(true);
            */
            
            warningIndicator[i].target = fakeTarget.transform;
            warningIndicator[i].duration = attackDelay;
            warningIndicator[i].gameObject.SetActive(true);
            
            // 지연 후 이펙트 생성 및 경고 제거
            StartCoroutine(SpawnRootAfterDelay(spawnPos, fakeTarget));
        }
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

        GameObject acidPrefab = Instantiate(effectPrefab, pos, Quaternion.identity, transform);
        MonsterProjectile acidEffect = acidPrefab.GetComponent<MonsterProjectile>();
        acidEffect.Initialize(Vector3.zero, damage);
        Destroy(acidEffect, 0.5f);
        Destroy(fakeTarget); // 타겟 오브젝트 삭제
    } // 
}
