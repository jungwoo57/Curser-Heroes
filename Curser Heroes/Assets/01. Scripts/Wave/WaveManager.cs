using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private CursorWeapon cursorWeapon;

    public StageData currentStage;
    public Spawner spawner;

    public static WaveManager Instance { get; private set; }
    public int CurrentWaveNumber => currentWaveIndex + 1;

    public bool IsWavePlaying => !waveCleared && spawningComplete;

    public int clearGold;
    public int clearJewel;

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
    }

    public void SetStage(StageData stage)
    {
        currentStage = stage;
        Debug.Log($"[WaveManager] 현재 스테이지 변경됨: {stage.stageNumber}");
        currentWaveIndex = 0;
    }

    [ContextMenu("스폰시키기")]
    public void StartWave()
    {
        if (currentStage == null)
        {
            Debug.LogError("[WaveManager] 스테이지가 설정되지 않았습니다.");
            return;
        }

        waveCleared = false;
        spawningComplete = false;
        cursorWeapon.ResetSweepCounter();

        int waveNum = currentWaveIndex + 1;
        Debug.Log($"웨이브 시작: {waveNum} (스테이지: {currentStage.stageNumber})");

        FindObjectOfType<BattleUI>()?.TextUpdate();

        if (WaveUtils.IsBossWave(waveNum))
        {
            SpawnBoss(currentStage.boss);
        }
        else
        {
            var spawnQueue = WaveBuilder.BuildWave(waveNum, currentStage.monsterPool);
            SpawnMonsters(spawnQueue);
        }

        spawningComplete = true;
        TriggerPassiveSkills();
    }

    private void SpawnBoss(BossData bossData)
    {
        spawnedMonsters.Clear();

        if (bossData == null || bossData.BossPrefab == null)
        {
            Debug.LogWarning("[WaveManager] 보스 데이터가 없습니다.");
            return;
        }

        GameObject bossObj = Instantiate(bossData.BossPrefab, Vector3.zero, Quaternion.identity);
        spawnedMonsters.Add(bossObj);

        BaseMonster boss = bossObj.GetComponent<BaseMonster>();
        if (boss != null)
        {
            boss.onDeath += OnMonsterKilled;
        }
    }

    private void SpawnMonsters(List<MonsterData> monsters)
    {
        spawnedMonsters = spawner.SpawnMonsters(monsters, OnMonsterKilled);

        foreach (var monsterObj in spawnedMonsters)
        {
            BaseMonster monster = monsterObj.GetComponent<BaseMonster>();
            if (monster != null)
            {
                monster.onDeath += OnMonsterKilled;
            }
        }
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
        if (!spawnedMonsters.Contains(monster))
        {
            Debug.LogWarning($"[OnMonsterKilled] 리스트 외 몬스터: {monster.name}");
            return;
        }

        Debug.Log($"[WaveManager] 몬스터 사망 처리: {monster.name}");

        spawnedMonsters.Remove(monster);

        if (!spawningComplete || waveCleared) return;

        // 0.8초 후에 몬스터 수 다시 체크
        StartCoroutine(CheckWaveClearAfterDelay());
    }

    private IEnumerator CheckWaveClearAfterDelay()
    {
        yield return new WaitForSeconds(0.8f); // 분열 몬스터 등록 대기

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
        int waveNum = currentWaveIndex + 1;

        clearGold = WaveUtils.CalculateGoldReward(waveNum);
        GameManager.Instance.AddGold(clearGold);

        int? jewel = WaveUtils.TryGetJewelReward(waveNum);
        if (jewel.HasValue)
        {
            clearJewel = jewel.Value;
            GameManager.Instance.AddJewel(jewel.Value);
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
        if (GameManager.Instance.bestScore < currentWaveIndex)
        {
            GameManager.Instance.bestScore = currentWaveIndex;
        }
    }

    public void RegisterMonster(GameObject monster)
    {
        if (!spawnedMonsters.Contains(monster))
        {
            spawnedMonsters.Add(monster);
        }
    }
}