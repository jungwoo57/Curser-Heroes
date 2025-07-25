

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
   [SerializeField] private float jumpAttackDelay;  // 점프 공격 딜레이
   [SerializeField] private float areaMoveTime;  //경고 범위 따라다니는 시간
   [SerializeField] private GameObject warningArea;
   [SerializeField] private float jumpAttackRange;
   float jumpTime = 0.5f;  // 날아오르는 시간


   override protected void Awake()
   {
      base.Awake();
      warningArea.SetActive(false);
   }
   
   private void PatternAttack()
   {
      
   }
   
   IEnumerator Pattern1() // 덤벨공격
   {
      yield return new WaitForSeconds(dumbbelAttackDelay);
      DumbbellAttack();
      yield return new WaitForSeconds(dumbbelAttackDelay);
      DumbbellAttack();
   }

   IEnumerator Pattern2()
   {
      //애니메이터 bool혹은 트리거 값 추가
      yield return new WaitForSeconds(jumpAttackDelay);
      StartCoroutine(JumpAttack());
      yield return new WaitForSeconds(jumpAttackDelay);
      StartCoroutine(JumpAttack());
   }

   private void Pattern3()
   {
      
   }

   private void DumbbellAttack()
   {
      if (dumbbell != null && firePoint != null)
      {
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
      float areaMoveTIme = 0;
      Vector3 startPos = transform.position;
      Vector3 flyPos = transform.position + Vector3.up * jumpDistance;  // 올라갈 위치
      while (elapsedTime < jumpTime)
      {
         transform.position = Vector3.Lerp(startPos, flyPos, elapsedTime/jumpTime);
         elapsedTime += Time.deltaTime;
         yield return null;
      }

      elapsedTime = 0;
      warningArea.SetActive(true);
      Collider2D warningCol = warningArea.GetComponent<Collider2D>();
      while (elapsedTime <= areaMoveTIme)   // 경고범위가 따라다니는 시간
      {
         CheckTarget();
         warningArea.transform.position = targetPos;
         elapsedTime+= Time.deltaTime;
         yield return null;
      }
      warningCol.enabled = true;  // warningArea에서 공격 처리
      yield return new WaitForSeconds(0.3f); // 대기시간
      warningCol.enabled = false;
      warningArea.SetActive(false);
   }
   

}
