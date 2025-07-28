
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class HealthEagleBoss : BossBase
{
   [Header("덤벨 공격 관련")]
   public GameObject dumbbell;  // 아령발사체 프리펩
   [SerializeField] private float dumbbelSpeed; // 덤벨 날아가는 속도
   [SerializeField] private float dumbbelAttackDelay;
   [SerializeField] private Transform firePoint; // 발사 위치
   
   [Header("점프 공격 관련")]
   [SerializeField] private float jumpDistance; // 점프 높이
   [SerializeField] private float fallTime;
   [SerializeField] private float jumpAttackDelay;  // 점프 공격 딜레이
   [SerializeField] private float areaMoveTime;  //경고 범위 따라다니는 시간
   [SerializeField] private GameObject warningArea;
   [SerializeField] private float jumpAttackRange;
   float jumpTime = 0.5f;  // 날아오르는 시간

   [Header("바람공격 관련")] 
   [SerializeField] private GameObject bossWind; // 바람 프리팹
   [SerializeField] private float windSpeed;  // 바람 속도
   [SerializeField] private float windDuration;  // 바람 지속시간
   [SerializeField] private float windWaitTime; // 전조 시간

   [Header("쿨다운")] 
   [SerializeField] private float[] curCoolDown;

   [SerializeField] private float patternWaitTime;
   
   override protected void Awake()
   {
      base.Awake();
      warningArea.SetActive(false);
      bossWind.SetActive(false);
   }

   private void Start()
   {
      patterns.Add(() => StartCoroutine(Pattern1()));
      patterns.Add(() => StartCoroutine(Pattern2()));
      patterns.Add(() => StartCoroutine(Pattern3()));
   }
   private void Update()
   {
      if (isDie) return;
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
         int i =Random.Range(0, canPatterns.Count);
         int index = canPatterns[i];
         patterns[index]?.Invoke();
      }
      
   }
   
   IEnumerator Pattern1() // 덤벨공격
   {
      curCoolDown[0] = 0;
      isPattern = true; //패턴 시작
      yield return new WaitForSeconds(dumbbelAttackDelay);
      DumbbellAttack();
      yield return new WaitForSeconds(dumbbelAttackDelay);
      DumbbellAttack();
      yield return new WaitForSeconds(patternWaitTime);
      curCoolDown[0] = 0;
      isPattern = false; // 패턴 종료
   }

   IEnumerator Pattern2()
   {
      isPattern = true;
      //애니메이터 bool혹은 트리거 값 추가
      yield return StartCoroutine(JumpAttack());
      yield return new WaitForSeconds(jumpTime);
      yield return StartCoroutine(JumpAttack());
      //StartCoroutine(JumpAttack());
      yield return new WaitForSeconds(patternWaitTime);
      curCoolDown[1] = 0;
      isPattern = false;
   }
   
   IEnumerator Pattern3()
   {
      isPattern = true;
      yield return null;
      yield return StartCoroutine(WindAttack());
      yield return new WaitForSeconds(patternWaitTime);
      curCoolDown[2] = 0; // 쿨타임 초기화
      isPattern = false;
   }

   private void DumbbellAttack()
   {
      if (dumbbell != null && firePoint != null)
      {
         Debug.Log("덤벨공격");
         CheckTarget();
         Vector3 direction = (targetPos - firePoint.position).normalized;
         GameObject projectile = Instantiate(dumbbell, firePoint.position, Quaternion.identity);
         MonsterProjectile projScript = projectile.GetComponent<MonsterProjectile>();
         if (projScript != null)
            projScript.Initialize(direction, 1);
      }
   }

   IEnumerator JumpAttack()
   {
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
         Debug.Log("타겟 위치" + targetPos);
         warningArea.transform.position = targetPos;
         elapsedTime += Time.deltaTime;
         yield return null;
      }

      Vector3 fallPos = warningArea.transform.position;
      ///////////////////////
      elapsedTime = 0;
      startPos = transform.position;
      while (elapsedTime <= fallTime) // 떨어지기
      {
         warningArea.transform.position = fallPos;
         transform.position = Vector3.Lerp(startPos, fallPos, elapsedTime/fallTime);
         elapsedTime += Time.deltaTime;
         yield return null;
      }
      warningCol.enabled = true;  // warningArea에서 공격 처리
      yield return new WaitForSeconds(0.3f); // 판정 시간
      warningCol.enabled = false;
      warningArea.SetActive(false);
   }

   IEnumerator WindAttack()
   {
      yield return new WaitForSeconds(windWaitTime);
      bossWind.transform.position = firePoint.position; // 위치 초기화
      bossWind.SetActive(true);
      float elapsedTime = 0;
      while (elapsedTime <= windDuration)
      {
         CheckTarget();
         Vector3 dir = (targetPos - bossWind.transform.position).normalized;
         bossWind.transform.position +=  dir * windSpeed*Time.deltaTime;
         elapsedTime += Time.deltaTime;
         yield return null;
      } 
      bossWind.SetActive(false);
   }
}
