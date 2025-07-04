using UnityEngine;

public abstract class SubProjectile : MonoBehaviour
{
    protected SubWeaponData weaponData;
    protected Vector3 targetPosition;

    public virtual void Init(SubWeaponData data, Vector3 targetPos)
    {
        weaponData = data;
        targetPosition = targetPos;
    }




    protected void ApplyDamage(Monster monster)
    {
        int dmg = Mathf.RoundToInt(weaponData.GetDamage());
        monster.TakeDamage(dmg);
    }

    protected void ApplyEffect(Monster monster)
    {
        if (monster.TryGetComponent(out EffectManager effectManager))
        {
            IEffect effect = EffectFactory.CreateEffect(weaponData.effect);
            if (effect != null)
                effectManager.AddEffect(effect);
        }
    }
}
