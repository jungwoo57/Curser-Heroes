public static class EffectFactory
{
    public static IEffect CreateEffect(SubWeaponEffect effect)
    {
        switch (effect)
        {
            case SubWeaponEffect.Stun:
                return new StunEffect();
            case SubWeaponEffect.Burn:
                return new BurnEffect();
            default:
                return null;
        }
    }
}
