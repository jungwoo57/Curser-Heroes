using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private CursorWeapon cursorWeapon;

    public WaveGroupData waveGroupData;
    public GameManager gameManager;
    public Spawner spawner;
    public static WaveManager Instance { get; private set; }
    public bool IsWavePlaying => !waveCleared && spawningComplete;

    public int clearGold;
    public int clearJewel;
    public WaveEntry currentWaveData;

    private int currentWaveIndex = 0;
    private List<GameObject> spawnedMonsters = new List<GameObject>();
    private bool waveCleared = false;
    private bool spawningComplete = false;

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
        spawningComplete = false;

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
        spawningComplete = true;
        TriggerPassiveSkills();

        if (SkillManager.Instance.arcaneTrailSkillInstance != null)
        {
            var trailInstance = SkillManager.Instance.ownedSkills
                .FirstOrDefault(s => s.skill.skillName == "마법 잔상");

            if (trailInstance != null)
            {
                SkillManager.Instance.arcaneTrailSkillInstance.Init(trailInstance);
            }
        }
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
        Debug.Log($"[OnMonsterKilled] 몬스터 제거 요청: {monster.name}");

        if (!spawnedMonsters.Contains(monster))
        {
            Debug.LogWarning($"[OnMonsterKilled] 제거 대상이 리스트에 없습니다: {monster.name}");
        }

        spawnedMonsters.Remove(monster);
        Debug.Log($"[OnMonsterKilled] 남은 몬스터 수: {spawnedMonsters.Count}");

        if (!spawningComplete)
        {
            Debug.Log($"[OnMonsterKilled] 스폰 완료 전이라 무시");
            return;
        }

        if (!waveCleared && spawnedMonsters.Count == 0)
        {
            Debug.Log("[OnMonsterKilled] 모든 몬스터 제거됨 → 웨이브 클리어 처리 시작");
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
        if (gameManager.bestScore < currentWaveIndex)
        {
            gameManager.bestScore = currentWaveIndex;
        }
    }
    // 맨 마지막에 추가

    public void RegisterMonster(GameObject monster)
    {
        if (spawnedMonsters.Contains(monster)) return;

        spawnedMonsters.Add(monster);
        Debug.Log($"[RegisterMonster] 몬스터 등록됨: {monster.name} | 총 {spawnedMonsters.Count}마리 추적 중");
    }

}