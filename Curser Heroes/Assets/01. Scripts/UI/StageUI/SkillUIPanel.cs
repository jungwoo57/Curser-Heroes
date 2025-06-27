using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIPanel : MonoBehaviour
{
    [SerializeField] public SkillUI[] skills;
    [SerializeField] private SkillManager skillManager; //SkillManager 연결 필요

    public void SkillUpdate()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            if (i < skillManager.ownedSkills.Count)
            {
                skills[i].SetSkill(skillManager.ownedSkills[i]);
                skills[i].gameObject.SetActive(true);
            }
            else
            {
                skills[i].gameObject.SetActive(false);
            }
        }
    }
}
