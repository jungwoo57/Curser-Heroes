using UnityEngine;

public static class WaveUtils
{
    public static bool IsBossWave(int wave) => wave % 20 == 0;

    public static int GetWaveValue(int wave) => 30 + (wave - 1) * 2;

    public static int CalculateGoldReward(int wave, StageData stageData)
    {
        if (stageData == null)
        {
            Debug.LogWarning("StageData is null. Using default gold reward calculation.");
            return (wave * 10) + Random.Range(0, wave + 1); // 기본값
        }

        int baseReward = stageData.baseGoldReward;

        int waveValue = GetWaveValue(wave);
        return waveValue * baseReward + Random.Range(0, waveValue + 1);
    }

    public static int? TryGetJewelReward(int wave)
    {
        int waveValue = GetWaveValue(wave);
        return Random.value < 0.25f ? Random.Range(0, waveValue + 1) : null;
    }
}