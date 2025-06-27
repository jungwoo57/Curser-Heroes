using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SkillData;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private GameObject rewardSelectUIPrefab;

    public WaveManager waveManager;
    public List<SkillData> skillPool = new List<SkillData>(); // 12개 선택된 스킬
    public List<SkillInstance> ownedSkills = new List<SkillInstance>(); // 보유 중인 스킬

    public int maxSkillCount = 6;

    public GameObject skillSelectUIPrefab;

    public void OnWaveEnd()
    {
        // 모두 최대레벨이라면 리워드 선택으로
        if (ownedSkills.Count == maxSkillCount && ownedSkills.All(s => s.IsMaxed))
        {
            ShowRewardSelection(); // 목숨, 골드, 쥬얼 중 하나 선택
        }
        else
        {
            ShowSkillSelection(); // 스킬 중 하나 선택
        }
    }

    void ShowSkillSelection()
    {
        List<SkillData> availableSkills = skillPool
            .Where(skill =>
                !ownedSkills.Any(own => own.skill == skill && own.IsMaxed) &&
                (ownedSkills.Count < maxSkillCount || ownedSkills.Any(own => own.skill == skill))
            )
            .ToList();

        List<SkillData> selection = GetRandomSkills(availableSkills, 3);
        GameObject ui = Instantiate(skillSelectUIPrefab);
        ui.GetComponent<SkillSelectUI>().Show(selection, OnSkillSelected);
    }

    void OnSkillSelected(SkillData selected)
    {
        var owned = ownedSkills.FirstOrDefault(s => s.skill == selected);
        if (owned != null)
            owned.level++;
        else
            ownedSkills.Add(new SkillInstance { skill = selected, level = 1 });

        waveManager.StartWave(); // 선택 끝 → 다음 웨이브 시작
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
        */

        waveManager.StartWave();
    }

    List<SkillData> GetRandomSkills(List<SkillData> source, int count)
    {
        return source.OrderBy(x => Random.value).Take(count).ToList();
    }
}
