using System.Collections.Generic;
using System;
using UnityEngine;

public class SkillSelectUI : MonoBehaviour
{
    public SkillSlot[] slots;

    public void Show(List<SkillData> skills, Action<SkillData> onSelect)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < skills.Count)
            {
                var skill = skills[i];
                int ownedLevel = SkillManager.Instance.GetSkillLevel(skill); // 현재 보유 레벨
                slots[i].Set(skill, ownedLevel, () => onSelect?.Invoke(skill));
                slots[i].gameObject.SetActive(true);
            }
            else
            {
                slots[i].gameObject.SetActive(false);
            }
        }
    }
}