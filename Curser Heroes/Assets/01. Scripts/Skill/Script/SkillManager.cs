using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private GameObject rewardSelectUIPrefab;
    [SerializeField] private GameObject skillSelectUIPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private CursorWeapon cursorWeapon;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject explodeOnKillSkillPrefab;

    public static SkillManager Instance { get; private set; }
    public List<SkillData> skillPool = new List<SkillData>();
    public List<SkillInstance> ownedSkills = new List<SkillInstance>();

    public CursorWeapon CursorWeapon => cursorWeapon;
    public int maxSkillCount = 6;

    private GameObject skillSelectUIInstance;
    private Dictionary<SkillData, GameObject> persistentSkillObjects = new Dictionary<SkillData, GameObject>();

    // ExplodeOnKillSkill 인스턴스 캐싱
    private ExplodeOnKillSkill explodeSkillComponent;

    [System.Serializable]
    public class SkillInstance
    {
        public SkillData skill;
        public int level;
        public bool IsMaxed => level >= skill.maxLevel;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        ExplodeOnKillSkill.explosionPrefab = explosionPrefab;

        // 자동 생성
        if (FindObjectOfType<ExplodeOnKillSkill>() == null && explodeOnKillSkillPrefab != null)
        {
            Instantiate(explodeOnKillSkillPrefab);
        }
    }

    void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance가 null입니다.");
            return;
        }
        // skillPool 동기화
        skillPool = new List<SkillData>(GameManager.Instance.skillPool);

        Debug.Log($"[SkillManager] skillPool 크기: {skillPool.Count}");
    }

    public void OnWaveEnd()
    {
        if (ownedSkills.Count == maxSkillCount && ownedSkills.All(s => s.IsMaxed))
        {
            ShowRewardSelection();
        }
        else
        {
            ShowSkillSelection();
        }
    }

    public bool HasSkill(string skillName)
    {
        return ownedSkills.Any(s => s.skill.skillName == skillName);
    }

    void ShowSkillSelection()
    {
        if (skillSelectUIPrefab == null)
        {
            Debug.LogError("SkillSelectUIPrefab이 연결되어 있지 않습니다.");
            return;
        }

        skillSelectUIInstance = Instantiate(skillSelectUIPrefab);
        var canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            skillSelectUIInstance.transform.SetParent(canvas.transform, false);
        }

        var skillUI = skillSelectUIInstance.GetComponent<SkillSelectUI>();
        if (skillUI == null)
        {
            Debug.LogError("SkillSelectUI 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        List<SkillData> availableSkills = skillPool
            .Where(skill =>
                !ownedSkills.Any(own => own.skill == skill && own.IsMaxed) &&
                (ownedSkills.Count < maxSkillCount || ownedSkills.Any(own => own.skill == skill))
            ).ToList();

        List<SkillData> selection = GetRandomSkills(availableSkills, 3);
        skillUI.Show(selection, OnSkillSelected);
    }

    void OnSkillSelected(SkillData selected)
    {
        var owned = ownedSkills.FirstOrDefault(s => s.skill == selected);
        if (owned != null)
        {
            owned.level++;
        }
        else
        {
            owned = new SkillInstance { skill = selected, level = 1 };
            ownedSkills.Add(owned);
        }

        // 레벨업 또는 신규 습득 후 자동 배치
        if (selected.skillName == "매직소드" || selected.skillName == "포이즌필드" || selected.skillName == "수호의 방패")
        {
            DeployPersistentSkill(owned);
        }

        Debug.Log($"[SkillManager] 스킬 선택됨: {selected.skillName}");

        if (UIManager.Instance == null)
        {
            Debug.LogError("UIManager.Instance is null!");
        }
        else if (UIManager.Instance.battleUI == null)
        {
            Debug.LogError("UIManager.Instance.battleUI is null!");
        }
        else
        {
            Debug.Log("[SkillManager] SkillUpdate 호출");
            UIManager.Instance.battleUI.SkillUpdate();
        }

        Destroy(skillSelectUIInstance);
        skillSelectUIInstance = null;

        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.IncrementWaveIndex();
            WaveManager.Instance.StartWave();
        }
        else
        {
            Debug.LogError("WaveManager.Instance가 null입니다.");
        }
    }

    public void DeployPersistentSkill(SkillInstance skillInstance)
    {
        if (cursorWeapon == null)
        {
            Debug.LogWarning("CursorWeapon 참조가 없습니다.");
            return;
        }

        SkillData skillData = skillInstance.skill;

        if (persistentSkillObjects.TryGetValue(skillData, out GameObject existingObj))
        {
            Debug.Log($"[SkillManager] 지속형 스킬 이미 설치됨: {skillData.skillName}");

            if (existingObj.TryGetComponent(out RotatingSkill rotating))
            {
                rotating.UpdateSwords(skillInstance);
            }
            else if (existingObj.TryGetComponent(out AoEFieldSkill aoe))
            {
                aoe.Init(skillInstance, cursorWeapon.transform);
            }
            else if (existingObj.TryGetComponent(out RotatingShieldSkill shield))
            {
                shield.UpdateShields(skillInstance);
            }
            return;
        }

        Vector3 spawnPos = cursorWeapon.transform.position;

        GameObject obj = Instantiate(skillData.skillPrefab, spawnPos, Quaternion.identity);
        persistentSkillObjects[skillData] = obj;

        if (obj.TryGetComponent(out RotatingShieldSkill shieldSkill))
        {
            Debug.Log("[SkillManager] 수호의 방패 Init 호출됨");
            shieldSkill.Init(skillInstance, cursorWeapon.transform);
        }
        else if (obj.TryGetComponent(out RotatingSkill rotatingSkill))
        {
            rotatingSkill.Init(skillInstance, cursorWeapon.transform);
        }
        else if (obj.TryGetComponent(out AoEFieldSkill aoeSkill))
        {
            aoeSkill.Init(skillInstance, cursorWeapon.transform);
        }
        else
        {
            Debug.LogWarning($"[SkillManager] {skillData.skillName}에 맞는 SkillBehaviour가 없습니다.");
        }
    }

    public void OnMonsterKilled(Vector3 deathPosition)
    {
        SkillInstance explodeSkill = ownedSkills.Find(s => s.skill.skillName == "장렬한 퇴장");
        if (explodeSkill == null) return;

        SkillLevelData data = explodeSkill.skill.levelDataList[explodeSkill.level - 1];
        int damage = data.damage;
        float radius = 1.5f;

        if (explodeSkillComponent != null)
        {
            explodeSkillComponent.TriggerExplosion(deathPosition, damage, radius, LayerMask.GetMask("Monster"));
        }
        else
        {
            Debug.LogWarning("ExplodeOnKillSkill 컴포넌트가 없습니다!");
        }
    }

    void ShowRewardSelection()
    {
        GameObject canvas = GameObject.Find("Canvas"); 
        if (canvas == null)
        {
            Debug.LogError("Canvas를 찾을 수 없습니다.");
            return;
        }

        GameObject ui = Instantiate(rewardSelectUIPrefab, canvas.transform, false);
        ui.GetComponent<RewardSelectUI>().Init(OnRewardSelected);
    }

    void OnRewardSelected(int index)
    {
        switch (index)
        {
            //case 0: BattleUI.Heal(); break;
            case 1: GameManager.Instance.AddGold(100); break;
            case 2: GameManager.Instance.AddJewel(10); break;
        }
        WaveManager.Instance.IncrementWaveIndex();
        WaveManager.Instance.StartWave();
    }

    List<SkillData> GetRandomSkills(List<SkillData> source, int count)
    {
        return source.OrderBy(x => Random.value).Take(count).ToList();
    }

    public void TrySpawnMeteorSkill(SkillInstance skillInstance)
    {
        float procChance = 0.25f;

        if (Random.value > procChance)
            return;

        List<Transform> monsters = new List<Transform>();

        foreach (var monster in FindObjectsOfType<BaseMonster>())
            monsters.Add(monster.transform);

        if (monsters.Count == 0)
            return;

        Transform target = monsters[Random.Range(0, monsters.Count)];

        GameObject meteorPrefab = skillInstance.skill.skillPrefab;
        GameObject meteorObj = Instantiate(meteorPrefab);

        var meteor = meteorObj.GetComponent<MeteorSkill>();
        var levelData = skillInstance.skill.levelDataList[skillInstance.level - 1];
        meteor.Init(levelData.damage, target.position);
    }
}