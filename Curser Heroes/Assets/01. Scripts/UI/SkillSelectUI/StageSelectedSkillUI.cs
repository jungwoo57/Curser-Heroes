using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StageSelectedSkillUI : MonoBehaviour
{
   public SkillData[] skills;
   public SkillUIImage[] skillImages;
   public Button applyButton;
   public bool isChange = false;
   private SkillPanelUI skillPanelUI;

   private void OnEnable()
   {
      isChange = false;
   }

   private void Start()
   {
      skillPanelUI = GetComponentInChildren<SkillPanelUI>();
   }

   public void SelectSkill(SkillData skill)       //스킬창에 고른 스킬 보여주기
   {
      isChange = true;
      for (int i = 0; i < skillImages.Length; i++)
      {
         if (skills[i] == skill)
         {
            skillImages[i].CancleSelect();
            return;
         }
         if (!skillImages[i].data)
         {
            skillImages[i].UpdateUI(skill);
            skills[i] = skillImages[i].data;
            return;
         }
      }
   }

   public void OnClickApplyButton()        //위치 변경 예정
   {
      for (int i = 0; i < skills.Length; i++)   //스킬 12개 이하면 리턴
      {
         if (!skills[i])
         {
            Debug.Log("12개의 스킬이 선택되지 않았습니다");
            return;
         }
      }
      GameManager.Instance.EquipSkill(skills);
      isChange = false;
   }

   public void ClearData()
   {
      for (int i = 0; i < skills.Length; i++)
      {
         skills[i] = null;
         skillImages[i].CancleSelect();
      }
   }
}
