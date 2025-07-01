using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private GameObject rewardSelectUIPrefab;
    [SerializeField] private GameObject skillSelectUIPrefab;

    public List<SkillData> allSkills;
    public List<SkillData> skillPool = new List<SkillData>();
    public List<SkillInstance> ownedSkills = new List<SkillInstance>();

    public int maxSkillCount = 6;

    private GameObject skillSelectUIInstance;

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
            owned.level++;
        else
            ownedSkills.Add(new SkillInstance { skill = selected, level = 1 });

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

        WaveManager.Instance.IncrementWaveIndex();
        WaveManager.Instance.StartWave();
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
    void StartWave()
    {
        foreach (var skill in skillManager.ownedSkills)
        {
            if (skill.skill.skillName == "매직 소드")
            {
                var obj = Instantiate(skill.skill.skillPrefab, player.transform.position, Quaternion.identity);
                obj.GetComponent<RotatingSkill>().Init(skill);
            }
        }
    }
}
