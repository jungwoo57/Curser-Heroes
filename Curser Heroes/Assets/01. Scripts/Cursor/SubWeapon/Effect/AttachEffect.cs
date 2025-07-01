using UnityEngine;

public class AttachEffectSubWeapon : MonoBehaviour
{
    public float attachDuration = 1.5f;

    public void ApplyEffect(BaseMonster target, float damage)
    {
        target.TakeDamage(Mathf.RoundToInt(damage));
        //target.ApplySticky(attachDuration); // 몬스터 움직임을 멈추는 식
    }
}
