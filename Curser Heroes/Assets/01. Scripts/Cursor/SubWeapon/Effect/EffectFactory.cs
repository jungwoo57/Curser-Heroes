using UnityEngine;

public static class EffectFactory
{
    public static IEffect CreateEffect(SubWeaponEffect effect)
    {
        switch (effect)
        {
            case SubWeaponEffect.Stun:
            // return new StunEffect();
            case SubWeaponEffect.Burn:
                return new BurnEffect();
            default:
                return null;
        }
    }
    //포스 이펙트 전용 오버로드
    public static IEffect CreateEffect(SubWeaponEffect effect, Vector3 centerPos, int damage, float radius, LayerMask targetLayer)
    {
        if (effect == SubWeaponEffect.Force)
        {
            return new ForceEffect(centerPos, damage, radius, targetLayer);
        }

        return null;
    }



}
