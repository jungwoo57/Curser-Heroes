using UnityEngine;

public static class WaveUtils
{
    public static bool IsBossWave(int wave) => wave % 20 == 0;

    public static int GetWaveValue(int wave) => 30 + (wave - 1) * 2;

    public static int CalculateGoldReward(int wave)
    {
        int waveValue = GetWaveValue(wave);
        return waveValue * 10 + Random.Range(0, waveValue + 1);
    }

    public static int? TryGetJewelReward(int wave)
    {
        int waveValue = GetWaveValue(wave);
        return Random.value < 0.25f ? Random.Range(0, waveValue + 1) : null;
    }
}