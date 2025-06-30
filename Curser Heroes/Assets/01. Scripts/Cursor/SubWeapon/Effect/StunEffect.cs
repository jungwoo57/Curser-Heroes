using UnityEngine;

public class StunEffect : MonoBehaviour, ISubWeaponEffect
{
    public float stunDuration = 2f;

    public void ApplyEffect(BaseMonster target, float damage)
    {
        target.TakeDamage(Mathf.RoundToInt(damage));
        //target.ApplyStun(stunDuration);
    }
}
