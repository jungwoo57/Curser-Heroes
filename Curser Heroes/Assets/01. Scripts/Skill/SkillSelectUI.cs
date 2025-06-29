using System;
using System.Collections.Generic;
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
                slots[i].Set(skill, () => onSelect?.Invoke(skill));
                slots[i].gameObject.SetActive(true);
            }
            else
            {
                slots[i].gameObject.SetActive(false);
            }
        }
    }
}