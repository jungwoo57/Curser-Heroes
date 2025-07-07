
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private SkillManager skillManager;
    [SerializeField] private CursorWeapon cursorWeapon;

    public WaveGroupData waveGroupData;
    public GameManager gameManager;
    public Spawner spawner;
    public static WaveManager Instance { get; private set; }

    public int clearGold;
    public int clearJewel;
    //public PoolSpawnerTest spawner; // Inspector에서 연결 필요

    public WaveEntry currentWaveData;
    private int currentWaveIndex = 0;
    private List<GameObject> spawnedMonsters = new List<GameObject>();
    private bool waveCleared = false; // 중복 웨이브 클리어 방지

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 중복 방지
            return;
        }

        Instance = this;
        gameManager  = GameManager.Instance;
    }

    [ContextMenu("스폰시키기")]
    public void StartWave()
    {
        Debug.Log($"[WaveManager] StartWave() 호출됨, currentWaveIndex = {currentWaveIndex}");
        if(currentWaveIndex > gameManager.bestScore) gameManager.bestScore = currentWaveIndex;
        waveCleared = false; // 새 웨이브 시작 시 초기화

        WaveEntry matched = waveGroupData.waveEntries.Find(w => w.wave == currentWaveIndex + 1);

        if (matched != null)
        {
            currentWaveData = new WaveEntry
            {
                overrideEnemies = matched.overrideEnemies,
                wave = matched.wave,
                forceExactOverride = matched.forceExactOverride
            };
        }
        else
        {
            currentWaveData = GenerateDynamicWaveEntry(currentWaveIndex + 1);
        }

        Debug.Log($"웨이브 시작: {currentWaveData.wave}");
        FindObjectOfType<BattleUI>()?.TextUpdate();

        var spawnQueue = WaveBuilder.BuildWaveEntry(currentWaveData, waveGroupData.globalMonsterPool);
        SpawnMonsters(spawnQueue);

        TriggerPassiveSkills();
    }
    private void TriggerPassiveSkills()
    {
        Debug.Log("[WaveManager] TriggerPassiveSkills() 호출됨");

        if (cursorWeapon == null)
        {
            Debug.LogWarning("CursorWeapon 참조가 없습니다.");
            return;
        }

        Vector3 playerPos = cursorWeapon.transform.position;

        foreach (var skill in skillManager.ownedSkills)
        {
            if (skill.skill.skillPrefab == null)
            {
                Debug.LogError($"[TriggerPassiveSkills] '{skill.skill.skillName}' 스킬의 skillPrefab이 할당되어 있지 않습니다!");
                continue;
            }

            switch (skill.skill.skillName)
            {
                case "매직소드":
                    Debug.Log($"[WaveManager] 매직소드 재배치 요청");
                    skillManager.DeployPersistentSkill(skill);
                    break;

                case "포이즌필드":
                    Debug.Log($"[WaveManager] 포이즌필드 재배치 요청");
                    skillManager.DeployPersistentSkill(skill);
                    break;

                default:
                    Debug.Log($"[TriggerPassiveSkills] 다른 스킬: {skill.skill.skillName}, 처리 없음");
                    break;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            DebugCurrentStatus();
        }
    }

    void DebugCurrentStatus()
    {
        Debug.Log($"[상태 확인]");

        if (currentWaveData == null)
        {
            Debug.LogWarning("currentWaveData가 설정되지 않았습니다.");
            return;
        }

        Debug.Log($"현재 웨이브: {currentWaveData.wave} (인덱스: {currentWaveIndex})");
        Debug.Log($"WaveValue: {currentWaveData.WaveValue}");
        Debug.Log($"ValueRange: ±{2 + (currentWaveData.wave / 10)}");
        Debug.Log($"살아있는 몬스터 수: {spawnedMonsters.Count}");

        if (gameManager != null)
        {
            Debug.Log($"보유 골드: {gameManager.GetGold()}");
            Debug.Log($"보유 쥬얼: {gameManager.GetJewel()}");
        }
    }

    void SpawnMonsters(List<MonsterData> monsters)
    {
        spawnedMonsters = spawner.SpawnMonsters(monsters, OnMonsterKilled);
    }

    void OnMonsterKilled(GameObject monster)
    {
        Debug.Log($"[몬스터 사망 감지] {monster.name} / ID: {monster.GetInstanceID()}");

        GameObject toRemove = null;

        foreach (var m in spawnedMonsters)
        {
            if (m == monster) // 참조 직접 비교
            {
                toRemove = m;
                break;
            }
        }

        if (toRemove != null)
        {
            spawnedMonsters.Remove(toRemove);
            Debug.Log($"[제거 성공] 남은 몬스터 수: {spawnedMonsters.Count}");
        }
        else
        {
            Debug.LogWarning($"[제거 실패] 리스트에 없는 몬스터: {monster.name}");
        }

        if (!waveCleared && spawnedMonsters.Count == 0)
        {
            waveCleared = true; // 중복 방지
            Debug.Log("[웨이브 클리어]");
            StartCoroutine(DelayedWaveClear(2f)); // 2초 후 OnWaveCleared 호출
        }
    }

    IEnumerator DelayedWaveClear(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("[웨이브 클리어] 2초 딜레이 후 호출");
        OnWaveCleared();
    }

    void OnWaveCleared()
    {
        Debug.Log("[WaveManager] OnWaveCleared 호출됨");
        clearGold = currentWaveData.CalculateGoldReward();
        gameManager.AddGold(currentWaveData.CalculateGoldReward());
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

        skillManager.OnWaveEnd(); // → 내부에서 스킬 또는 보상 선택 UI 표시
    }

    public void IncrementWaveIndex()
    {
        currentWaveIndex++;
    }

    private WaveEntry GenerateDynamicWaveEntry(int waveNumber)
    {
        return new WaveEntry
        {
            wave = waveNumber,
            overrideEnemies = new List<MonsterData>()
        };
    }
}
