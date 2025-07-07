
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern2Logic : PatternLogicBase
{
    [Header("패턴 시작 전 지연 (초)")]
    public float startDelay = 1.0f;          // 패턴이 시작되기 전 기다릴 시간

    [Header("순간이동 감지 반경 (Unity 단위)")]
    public float detectionRadius = 10f;

    public override IEnumerator Execute(BossPatternController controller)
    {
        // 1) Weapon 레이어만 필터링하기 위한 LayerMask
        int weaponMask = LayerMask.GetMask("Weapon");

        // 2) 반경 내 모든 Collider2D 검색
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            controller.transform.position,
            detectionRadius,
            weaponMask
        );

        // 3) 가장 가까운 무기 찾기
        Transform nearest = null;
        float minDistSqr = float.MaxValue;
        foreach (var hit in hits)
        {
            float distSqr = (hit.transform.position - controller.transform.position).sqrMagnitude;
            if (distSqr < minDistSqr)
            {
                minDistSqr = distSqr;
                nearest = hit.transform;
            }
        }

        // 4) 대상이 있으면 보스를 해당 위치로 순간이동
        if (nearest != null)
        {
            yield return new WaitForSeconds(startDelay);
            controller.transform.position = nearest.position;
        }
        else
        {
            Debug.LogWarning("Pattern2Logic: Weapon 레이어 오브젝트를 찾지 못했습니다.");
        }

        // 즉시 이동이므로 추가 대기 없이 종료
        yield break;
    }
}