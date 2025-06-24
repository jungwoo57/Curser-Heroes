using UnityEngine;

public class TestMonster : BaseMonster
{
    public GameObject TestMonsterPrefab; // Inspector에서 지정
    public Transform TestMonsterTransform;       // 소환 위치

    protected override void Attack()
    {

    }

    protected override void PlayHitEffect()
    {
        Debug.Log($"{gameObject.name} 피격 이펙트");
    }

    protected override void Die()
    {
        Debug.Log($"{gameObject.name} 사망");
        base.Die();
    }
}
