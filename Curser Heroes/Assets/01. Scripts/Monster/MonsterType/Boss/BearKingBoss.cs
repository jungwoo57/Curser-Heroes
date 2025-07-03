using UnityEngine;
using System.Collections;

public class BearKingBoss : BossBaseMonster
{
    // Animator Trigger hashes (make sure these parameters in your Animator are of type Trigger)
    private static readonly int HashP1 = Animator.StringToHash("Pattern1");
    private static readonly int HashP2 = Animator.StringToHash("Pattern2");
    private static readonly int HashP3 = Animator.StringToHash("Pattern3");

    protected override IEnumerator Pattern1()
    {
        // 한 번만 재생할 Trigger 발동
        animator.SetTrigger(HashP1);

        // "Boss_Idle" 상태로 돌아올 때까지 대기
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_Idle"));
    }

    protected override IEnumerator Pattern2()
    {
        animator.SetTrigger(HashP2);
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_Idle"));
    }

    protected override IEnumerator Pattern3()
    {
        animator.SetTrigger(HashP3);
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_Idle"));
    }

    // 활성화된 HitBox 자식 Collider2D 가 Weapon 레이어와 충돌할 때 호출
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            WeaponManager.Instance?.TakeWeaponLifeDamage();
        }
    }
}
