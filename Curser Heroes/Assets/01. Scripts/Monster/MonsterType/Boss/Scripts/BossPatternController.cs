using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossPatternController : MonoBehaviour
{   
    public BossData data;                  

    private Animator animator;
    public BossPatternDamage[] patternDamage;  // 히트박스 스크립트 배열
    private float[] nextAvailableTime;         // 패턴별 다음 실행 가능 시간

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        patternDamage = GetComponentsInChildren<BossPatternDamage>();

        int count = data.patternCooldown.Length;
        nextAvailableTime = new float[count];

        // 처음엔 첫 패턴 실행 전까지 initialDelay 대기
        for (int i = 0; i < count; i++)
            nextAvailableTime[i] = Time.time + data.initialDelay;
    }

    private void Start()
    {
        StartCoroutine(PatternLoop());
    }

    private IEnumerator PatternLoop()
    {
        // 초기 지연: 보스 등장 후 첫 패턴 전 대기
        yield return new WaitForSeconds(data.initialDelay);

        while (true)
        {
            // 실행 가능(쿨타임이 지난) 패턴만 골라 리스트에 추가
            List<int> available = new List<int>();
            for (int i = 0; i < nextAvailableTime.Length; i++)
            {
                if (Time.time >= nextAvailableTime[i])
                    available.Add(i);
            }

            // 실행 가능 패턴이 없으면 짧게 대기 후 재시도
            if (available.Count == 0)
            {
                yield return new WaitForSeconds(0.1f);
                continue;
            }

            // 가능한 패턴 중 하나를 랜덤 선택
            int randIdx = available[Random.Range(0, available.Count)];
            string trigger = "Pattern" + (randIdx + 1);

            // 히트박스 활성화/비활성화
            foreach (var d in patternDamage) d.Deactivate();
            if (randIdx < patternDamage.Length)
                patternDamage[randIdx].Activate();

            // 애니메이터 트리거 발동
            animator.SetTrigger(trigger);

            //// 6) 선택된 패턴의 쿨타임 설정
            //float cd = data.patternCooldown[randIdx];
            //nextAvailableTime[randIdx] = Time.time + cd;

            //// 7) 쿨타임만큼 대기 (패턴 재생 시간 + 추가 대기)
            //yield return new WaitForSeconds(0);

            //// 8) 히트박스 비활성화, Idle 복귀
            //if (randIdx < patternDamage.Length)
            //    patternDamage[randIdx].Deactivate();
            //animator.SetTrigger("BossIdle");

        }
    }
}
