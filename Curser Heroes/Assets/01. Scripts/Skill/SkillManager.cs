using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private GameObject rewardSelectUIPrefab;
    [SerializeField] private GameObject skillSelectUIPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private CursorWeapon cursorWeapon;

    public List<SkillData> allSkills;
    public List<SkillData> skillPool = new List<SkillData>();
    public List<SkillInstance> ownedSkills = new List<SkillInstance>();

    public CursorWeapon CursorWeapon => cursorWeapon;
    public int maxSkillCount = 6;

    private GameObject skillSelectUIInstance;
    private Dictionary<SkillData, GameObject> persistentSkillObjects = new Dictionary<SkillData, GameObject>();

    [System.Serializable]
    public class SkillInstance
    {
        public SkillData skill;
        public int level;
        public bool IsMaxed => level >= skill.maxLevel;
    }

    void Start()
    {
        Debug.Log($"[SkillManager] 등록된 스킬 개수: {allSkills.Count}");
        skillPool = new List<SkillData>(allSkills);
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
        if (selected.skillName == "매직소드" || selected.skillName == "포이즌필드")
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

        //SkillData 객체를 키로 사용
        if (persistentSkillObjects.TryGetValue(skillData, out GameObject existingObj))
        {
            Debug.Log($"[SkillManager] 지속형 스킬 이미 설치됨: {skillData.skillName}");

            if (existingObj.TryGetComponent(out RotatingSkill rotating))
            {
                rotating.UpdateSwords(skillInstance); // 상태 갱신
            }

            return; // 새 오브젝트 생성 방지
        }

        Vector3 spawnPos = cursorWeapon.transform.position;

        GameObject obj = Instantiate(skillData.skillPrefab, spawnPos, Quaternion.identity);
        persistentSkillObjects[skillData] = obj;

        if (obj.TryGetComponent(out RotatingSkill newRotating))
        {
            newRotating.Init(skillInstance, cursorWeapon.transform);
        }
        else if (obj.TryGetComponent(out AoEFieldSkill aoe))
        {
            aoe.Init(skillInstance,cursorWeapon.transform);
        }
        else
        {
            Debug.LogWarning($"[SkillManager] {skillData.skillName}에 맞는 SkillBehaviour가 없습니다.");
        }
    }

    void ShowRewardSelection()
    {
        GameObject ui = Instantiate(rewardSelectUIPrefab);
        ui.GetComponent<RewardSelectUI>().Init(OnRewardSelected);
    }

    void OnRewardSelected(int index)
    {
        /*switch (index)
        {
            case 0: PlayerManager.Instance.Heal(); break;
            case 1: CurrencyManager.Instance.AddGold(100); break;
            case 2: CurrencyManager.Instance.AddJewel(10); break;
        }
        waveManager.StartWave();
        */
    }
    List<SkillData> GetRandomSkills(List<SkillData> source, int count)
    {
        return source.OrderBy(x => Random.value).Take(count).ToList();
    }
    public void TrySpawnMeteorSkill(SkillManager.SkillInstance skillInstance)
    {
        float procChance = 0.25f;

        if (Random.value > procChance)
            return;

        // 씬 내 몬스터 리스트 가져오기 (기본/보스 모두)
        List<Transform> monsters = new List<Transform>();

        foreach (var monster in FindObjectsOfType<BaseMonster>())
            monsters.Add(monster.transform);

        foreach (var boss in FindObjectsOfType<BossBaseMonster>())
            monsters.Add(boss.transform);

        if (monsters.Count == 0)
            return;

        // 랜덤 몬스터 선택
        Transform target = monsters[Random.Range(0, monsters.Count)];

        // 별똥별 생성
        GameObject meteorPrefab = skillInstance.skill.skillPrefab; 
        GameObject meteorObj = Instantiate(meteorPrefab);

        var meteor = meteorObj.GetComponent<MeteorSkill>();
        var levelData = skillInstance.skill.levelDataList[skillInstance.level - 1];
        meteor.Init(levelData.damage, target.position);
    }
}
