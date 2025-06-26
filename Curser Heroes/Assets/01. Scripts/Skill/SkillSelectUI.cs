using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSelectUI : MonoBehaviour
{
    /*public SkillSlot[] slots; // 3개의 UI 슬롯
    private Action<SkillData> onSelected;

    public void Show(List<SkillData> skills, Action<SkillData> onSelect)
    {
        onSelected = onSelect;

        // 슬롯 수 부족 시 자동 할당 (옵션)
        if (slots == null || slots.Length == 0)
            slots = GetComponentsInChildren<SkillSlot>();

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < skills.Count)
                slots[i].Set(skills[i], () => Select(skills[i]));
            else
                slots[i].gameObject.SetActive(false);
            }
        }

    void Select(SkillData skill)
    {
        onSelected?.Invoke(skill);
        Destroy(gameObject); // UI 닫기
    }*/
}