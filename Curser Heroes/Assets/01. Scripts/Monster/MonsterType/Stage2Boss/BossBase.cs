using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase : BossStats
{ 
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;
    //public static event Action<BossBase> OnAnyMonsterDamaged; << ???

    [SerializeField] protected bool isPattern;
    [SerializeField] protected bool isDie;
    [SerializeField] protected bool canMove = false; // 첫 패턴 후 true;
    
    protected Vector3 targetPos;
    protected List<System.Action> patterns = new List<System.Action>();


    

    protected virtual void Update()
    {
        BossMove();
    }
    // 외부에서 호출하여 데미지를 입힐 때 쓰는 함수
    
    // 투명도를 깜빡이며 피격 효과
   

    // 죽었을 때 호출
    [ContextMenu("사망")]
    public override void Die()
    {
        base.Die();
        isDie = true;
    }

    private IEnumerator DelayedDeath()
    {
        yield return new WaitForSeconds(3.5f); // 연출 대기 시간

        WaveManager.Instance?.OnMonsterKilled(this.gameObject);
        Destroy(gameObject); // 이제 오브젝트 제거
    }

    protected void CheckTarget()
    {
        Collider2D weaponCollider = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Weapon"));
        if (weaponCollider == null) return;
        targetPos = weaponCollider.transform.position;
    }

    protected void BossMove()
    {
        if (isPattern || isDie || !canMove)
        {
            animator.SetBool("BossMove",false);
            return;
        }

        CheckTarget();
        if (animator)
        {
            animator.SetBool("BossMove",true);
        }
        Vector3 moveDir = targetPos - transform.position;
        transform.position += moveDir.normalized*Time.deltaTime*moveSpeed;
    }
}
