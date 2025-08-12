using UnityEngine;

public static class EffectFactory
{
   
    public static IEffect CreateEffect(
        SubWeaponEffect effect,
        BaseMonster target,
        int burnDPS,
        float burnDuration,
        float stunDuration
    )
    {
        switch (effect)
        {
            case SubWeaponEffect.Burn:
                // target, DPS, 지속시간, 틱 인터벌(여기선 1초)
                return new BurnEffect(target, burnDPS, burnDuration, 1f);

            case SubWeaponEffect.Stun:
                // 스턴은 duration만 필요
                return new StunEffect(stunDuration);

            default:
                return null;
        }
    }

   
    public static IEffect CreateEffect(
        SubWeaponEffect effect,
        Vector3 centerPos,
        int damage,
        float radius,
        LayerMask targetLayer
    )
    {
        if (effect == SubWeaponEffect.Force)
            return new ForceEffect(centerPos, damage, radius, targetLayer);

        return null;
    }
}
