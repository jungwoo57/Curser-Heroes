using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//
public class BossPatternController : MonoBehaviour
{

    
    public BossData data;

    public PatternLogicBase[] patternLogics;   // 인스펙터에서 순서대로 연결

    private Animator animator;
    public BossPatternDamage[] patternDamage;  // 히트박스 스크립트 배열
    private float[] nextAvailableTime;         // 패턴별 다음 실행 가능 시간
    public bool IsDead { get; set; } = false;

    public bool IsInPattern { get; private set; }
    public bool IsInSpawn { get; private set; } // 보스가 스폰 중인지 여부
    public float[] hitboxStartDelays;
    
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

   public IEnumerator PatternLoop()
    {
        IsInSpawn = true;
        animator.SetTrigger("BossSpawn");
        
        // 초기 지연: 보스 등장 후 첫 패턴 전 대기
        yield return new WaitForSeconds(data.initialDelay);
        IsInSpawn = false;
        while (!IsDead)  // 보스가 살아있는 동안 패턴 반복
        {            
            IsInPattern = true;

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
           
            //패턴 중 하나를 랜덤 선택
            int randIdx = available[Random.Range(0, available.Count)];
            string trigger = "Pattern" + (randIdx + 1);

            animator.SetTrigger(trigger);

            StartCoroutine(HitboxDelayAndActivate(randIdx));

            if (patternLogics != null && randIdx < patternLogics.Length && patternLogics[randIdx] != null)
            {
                // 각 패턴 로직 스크립트의 Execute() 코루틴을 실행
                yield return StartCoroutine(patternLogics[randIdx].Execute(this));
            }

            //애니메이션 재생 완료까지 대기
            yield return WaitForAnimationEnd(trigger);
            // 히트박스 끄기
            foreach (var d in patternDamage) d.Deactivate();
            

            animator.SetTrigger("BossIdle");
            yield return new WaitForSeconds(0.5f);
            IsInPattern = false;

            // 공용 쿨타임만큼 대기 
            yield return new WaitForSeconds(data.allpatternCooldown);

            // 9) 이번 패턴의 다음 실행 가능 시간 갱신
            nextAvailableTime[randIdx] = Time.time + data.patternCooldown[randIdx];         
        }
        
    }


    // 특정 애니메이션 상태가 끝날 때까지 대기하는 유틸
    private IEnumerator WaitForAnimationEnd(string stateName, float timeout = 5f)
    {
        int layer = 0;
        float start = Time.time;

        // 1) 상태 진입 대기
        while (!animator.GetCurrentAnimatorStateInfo(layer).IsName(stateName))
        {
            if (Time.time - start > timeout)
            {
                yield break;
            }
            yield return null;
        }

        // 2) 재생 완료 대기
        while (animator.GetCurrentAnimatorStateInfo(layer).normalizedTime < 1f)
        {
            if (Time.time - start > timeout)
            {

                break;
            }
            yield return null;
        }
    }
    private IEnumerator HitboxDelayAndActivate(int index)
    {
        if (index >= hitboxStartDelays.Length || index >= patternDamage.Length)
            yield break;

        float delay = hitboxStartDelays[index];
        yield return new WaitForSeconds(delay);

        patternDamage[index].Activate();
    }

}
    





    
    

