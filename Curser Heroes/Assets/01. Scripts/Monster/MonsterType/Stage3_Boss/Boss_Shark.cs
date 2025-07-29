using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Shark : BossBase
{
    [Header("패턴1 공격")] 
    [SerializeField] private float rushTime;  // 돌진시간
    [SerializeField] private Collider2D rushCol; // 패턴 1,2 공통
    
    
    [Header("패턴2 공격")]
    [SerializeField] private float jumpDistance; // 점프 높이
    [SerializeField] private float fallTime;
    [SerializeField] private float jumpAttackDelay;  // 점프 공격 딜레이
    [SerializeField] private float areaMoveTime;  //경고 범위 따라다니는 시간
    [SerializeField] private GameObject warningArea;
    [SerializeField] private float jumpAttackRange;
    //[SerializeField] private GameObject jumpEffect;
    float jumpTime = 0.5f;  // 날아오르는 시간

    [Header("패턴3 공격")] 
    [SerializeField] private float meleeAttackDuration;
    
    [Header("쿨다운")] 
    [SerializeField] private float[] curCoolDown;
    [SerializeField] private float patternWaitTime;

    
    override protected void Awake()
    {
        base.Awake();
        warningArea.SetActive(false);
        rushCol.enabled = false;
    }
    
    private void Start()
    {
        patterns.Add(() => StartCoroutine(Pattern1()));
        patterns.Add(() => StartCoroutine(Pattern2()));
        patterns.Add(() => StartCoroutine(Pattern3()));
    }
    
    protected override void Update()
    {
        if (isDie) return;
        base.Update();
        for (int i = 0; i < curCoolDown.Length; i++)
        {
            curCoolDown[i] += Time.deltaTime;
        }
        RandomPattern();
    }
    
    private void RandomPattern() // 시간계산해서 처리해주기 고민좀해보자
    {
        if (isPattern) return;  // 패턴중이면 리턴
        List<int> canPatterns = new List<int>();
        for (int i = 0; i < curCoolDown.Length; i++)
        {
            if (curCoolDown[i] >= (float)data.patternCooldown[i])
            {
                canPatterns.Add(i);
            }
        }

        if (canPatterns.Count > 0)
        {
            canMove = true;
            int i =Random.Range(0, canPatterns.Count);
            int index = canPatterns[i];
            patterns[index]?.Invoke();
        }
      
    }
    
    IEnumerator Pattern1() // 덤벨공격
    {
        curCoolDown[0] = 0;
        isPattern = true; //패턴 시작
        animator.SetTrigger("Pattern1");
        StartCoroutine(RushAttack());
        yield return new WaitForSeconds(patternWaitTime);
        curCoolDown[0] = 0;
        isPattern = false; // 패턴 종료
    }

    IEnumerator Pattern2()
    {
        isPattern = true;
        yield return StartCoroutine(JumpAttack());
        yield return new WaitForSeconds(jumpTime);
        yield return StartCoroutine(JumpAttack());
        yield return new WaitForSeconds(patternWaitTime);
        yield return new WaitForSeconds(1.0f);
        curCoolDown[1] = 0;
        isPattern = false;
    }
   
    IEnumerator Pattern3()
    {
        isPattern = true;
        yield return null;
        yield return StartCoroutine(MeleeAttack());
        yield return new WaitForSeconds(patternWaitTime);
        yield return new WaitForSeconds(2.0f);
        curCoolDown[2] = 0; // 쿨타임 초기화
        isPattern = false;
    }

    IEnumerator RushAttack()
    {
        CheckTarget();
        Vector3 endPos = targetPos;
        Vector3 startPos = transform.position;
        
        float elapsedTime = 0;
        rushCol.gameObject.SetActive(true);
        rushCol.enabled = true;
        while (elapsedTime <= rushTime)
        {
            transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / rushTime));
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        rushCol.enabled = false;
        rushCol.gameObject.SetActive(false);
    }
    
    IEnumerator JumpAttack()
    {
        animator.SetTrigger("Pattern2_Jump");
        yield return new WaitForSeconds(jumpTime);
        //Vector3 jumpPos = jumpEffect.transform.position;
        float elapsedTime = 0;
        Vector3 startPos = transform.position;
        Vector3 flyPos = transform.position + Vector3.up * jumpDistance;  // 올라갈 위치
        while (elapsedTime <= jumpTime)
        {
            transform.position = Vector3.Lerp(startPos, flyPos, elapsedTime/jumpTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //////////////////// 경고 범위 관련
        elapsedTime = 0;
        warningArea.SetActive(true);
        Collider2D warningCol = warningArea.GetComponent<Collider2D>();
        while (elapsedTime <= areaMoveTime)   // 경고범위가 따라다니는 시간
        {
            CheckTarget();
            warningArea.transform.position = targetPos;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Vector3 fallPos = warningArea.transform.position;
        ///////////////////////
        elapsedTime = 0;
        startPos = transform.position;
        animator.SetTrigger("Pattern2_Attack");
        yield return null;
        animator.speed = 0;
        while (elapsedTime <= fallTime) // 떨어지기
        {
            warningArea.transform.position = fallPos;
            transform.position = Vector3.Lerp(startPos, fallPos, elapsedTime/fallTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        animator.speed = 1.0f;
        warningCol.enabled = true;  // warningArea에서 공격 처리
        yield return new WaitForSeconds(0.3f); // 판정 시간
        animator.SetTrigger("Idle");
        warningCol.enabled = false;
        warningArea.SetActive(false);
    }

    IEnumerator MeleeAttack()
    {
        rushCol.gameObject.SetActive(true);
        rushCol.enabled = true;
        float elapsedTime = 0;
        while(elapsedTime <= meleeAttackDuration)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        rushCol.enabled = false;
        rushCol.gameObject.SetActive(false);
        
    }
}
