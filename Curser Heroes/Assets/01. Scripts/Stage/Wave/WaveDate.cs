using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "ScriptableObjects/WaveData", order = 2)]
public class WaveData : ScriptableObject
{
    public int wave;
    public int waveValue;
    [Header("Possible Enemies in this stage")]
    public List<MonsterData> spawnableEnemies;

    public int GetValueRange()
    {
        return 2 + (wave / 10);   // 10웨이브마다 1 증가
    }

    public int CalculateGoldReward()
    {
        return (waveValue * 10) + Random.Range(0, waveValue + 1);
    }

    public int? TryGetJewelReward()
    {
        if (Random.value < 0.25f)
            return Random.Range(0, waveValue + 1);
        return null;
    }
}
