using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUIPanel : MonoBehaviour
{
    public SkillSlot[] skills; // 화면에 표시할 슬롯들 (예: 6개)
    public SkillUI[] skillSlots;
    public SkillManager skillManager;

    public void SkillUpdate()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            if (i < skillManager.ownedSkills.Count)
            {
                skills[i].gameObject.SetActive(true);
                // SkillInstance 타입 넘김
                skills[i].Set(skillManager.ownedSkills[i], null); // 콜백 없으면 null 가능
            }
            else
            {
                skills[i].gameObject.SetActive(false);
            }
        }
    }
    public void UpdateSkillUI(List<SkillManager.SkillInstance> ownedSkills)
    {
        for (int i = 0; i < skillSlots.Length; i++)
        {
            skillSlots[i].gameObject.SetActive(true);

            if (i < ownedSkills.Count)
            {
                skillSlots[i].SetSkill(ownedSkills[i]);
            }
            else
            {
                skillSlots[i].SetEmpty();
            }
        }
    }
}
