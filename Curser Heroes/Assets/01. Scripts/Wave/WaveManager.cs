using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private CursorWeapon cursorWeapon;

    public WaveGroupData waveGroupData;
    public GameManager gameManager;
    public Spawner spawner;
    public static WaveManager Instance { get; private set; }

    public int clearGold;
    public int clearJewel;
    public WaveEntry currentWaveData;

    private int currentWaveIndex = 0;
    private List<GameObject> spawnedMonsters = new List<GameObject>();
    private bool waveCleared = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        gameManager = GameManager.Instance;
    }

    [ContextMenu("스폰시키기")]
    public void StartWave()
    {
        waveCleared = false;

        cursorWeapon.ResetSweepCounter();

        int waveNum = currentWaveIndex + 1;
        WaveEntry matched = waveGroupData.waveEntries.Find(w => w.wave == waveNum);

        currentWaveData = matched != null ? matched : new WaveEntry { wave = waveNum };

        Debug.Log($"웨이브 시작: {currentWaveData.wave}");

        FindObjectOfType<BattleUI>()?.TextUpdate();

        if (currentWaveData.isBossWave && currentWaveData.HasBosses)
        {
            SpawnBosses(currentWaveData.overrideBosses);
        }
        else
        {
            var spawnQueue = WaveBuilder.BuildWaveEntry(currentWaveData, waveGroupData.globalMonsterPool);
            SpawnMonsters(spawnQueue);
        }

        TriggerPassiveSkills();
    }

    // 소환된 몬스터 리스트 초기화 및 추가,보스 데이터가 있으면 소환 
    private void SpawnBosses(List<BossData> bosses)
    {
        spawnedMonsters.Clear();
        foreach (var bossData in bosses)
        {
            if (bossData == null || bossData.BossPrefab == null) continue;

            GameObject bossObj = Instantiate(bossData.BossPrefab, Vector3.zero, Quaternion.identity);
            spawnedMonsters.Add(bossObj);
        }
    }

    private void SpawnMonsters(List<MonsterData> monsters)
    {
        spawnedMonsters = spawner.SpawnMonsters(monsters, OnMonsterKilled);
    }

    private void TriggerPassiveSkills()
    {
        foreach (var skill in SkillManager.Instance.ownedSkills)
        {
            if (skill.skill.skillPrefab == null) continue;

            switch (skill.skill.skillName)
            {
                case "매직소드":
                case "포이즌필드":
                case "수호의 방패":
                    SkillManager.Instance.DeployPersistentSkill(skill);
                    break;
            }
        }
    }

    public void OnMonsterKilled(GameObject monster)
    {
        spawnedMonsters.Remove(monster);
        if (!waveCleared && spawnedMonsters.Count == 0)
        {
            waveCleared = true;
            StartCoroutine(DelayedWaveClear(1.5f));
        }
    }

    IEnumerator DelayedWaveClear(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnWaveCleared();
    }

    void OnWaveCleared()
    {
        clearGold = currentWaveData.CalculateGoldReward();
        gameManager.AddGold(clearGold);

        int? jewel = currentWaveData.TryGetJewelReward();
        if (jewel.HasValue)
        {
            gameManager.AddJewel(jewel.Value);
            clearJewel = jewel.Value;
        }
        else
        {
            clearJewel = 0;
        }

        SkillManager.Instance.OnWaveEnd();
    }

    public void IncrementWaveIndex()
    {
        currentWaveIndex++;
    }
}