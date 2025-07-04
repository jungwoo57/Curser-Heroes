public static class SubWeaponUtils
{
    public static float GetSpeed(SubProjectileSpeed speed)
    {
        return speed switch
        {
            SubProjectileSpeed.Slow => 4f,
            SubProjectileSpeed.Medium => 7f,
            SubProjectileSpeed.Fast => 10f,
            _ => 6f
        };
    }
}
