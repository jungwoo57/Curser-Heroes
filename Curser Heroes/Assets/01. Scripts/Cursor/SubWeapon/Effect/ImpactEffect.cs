using UnityEngine;

public class ImpactEffect : MonoBehaviour, ISubWeaponEffect
{
    public void ApplyEffect(BaseMonster target, float damage)
    {
        target.TakeDamage(Mathf.RoundToInt(damage));
    }
}
