using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSkillSelectUI : MonoBehaviour
{
    public IReadOnlyList<SkillData> hasSkills;
    public List<SkillUIImage> showSkills = new List<SkillUIImage>();
    public SkillUIImage skillUIImage;
    public StageSelectedSkillUI stageSelectedSkillUI;
    
    private void Start()
    {
        hasSkills = GameManager.Instance.hasSkills;
        if (hasSkills != null)
        {
            UpdateUI();
        }
        else
        {
            Debug.Log("데이터 없음");
        }
    }

    public void UpdateUI()
    {
        while (showSkills.Count < hasSkills.Count)
        {
            SkillUIImage obj = Instantiate(skillUIImage ,this.transform);
            showSkills.Add(obj);
            Debug.Log("스킬 칸 동적 생성");
        }

        for (int i = 0; i < showSkills.Count; i++)         //이미지 업데이트
        {
            showSkills[i].gameObject.SetActive(true);
            showSkills[i].UpdateUI(hasSkills[i]);
        }
    }
}
