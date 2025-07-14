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
   public SkillData[] previousSkills;        //추가 코드
   
   private void OnEnable()
   {
      isChange = false;
      for (int i = 0; i < previousSkills.Length; i++)
      {
         previousSkills[i] = skills[i];
         skillImages[i].UpdateUI(previousSkills[i]);
      }

      if (GameManager.Instance.selectSkills.Count == skills.Length) //스킬들의 길이와 선택 스킬이 같으면
      {
         for (int i = 0; i < GameManager.Instance.selectSkills.Count; i++)
         {
            skills[i] = GameManager.Instance.selectSkills[i];
            skillImages[i].UpdateUI(skills[i]);
         }
      }
      InteractApplyButton();
   }

   private void Start()
   {
      skillPanelUI = GetComponentInChildren<SkillPanelUI>();
   }

   public void SelectSkill(SkillData skill)       //스킬창에 고른 스킬 보여주기
   {
      Debug.Log(skillImages.Length);
      isChange = true;
      for (int i = 0; i < skillImages.Length; i++)
      {
         if (skills[i] == skill)
         {
            skillImages[i].CancleSelect();
            skills[i] = null;
            InteractApplyButton();
            return;
         }
      }
      for (int i = 0; i < skillImages.Length; i++)
      {
         if (!skillImages[i].data)
         {
            skillImages[i].UpdateUI(skill);
            skills[i] = skillImages[i].data;
            InteractApplyButton();
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

      for (int i = 0; i < skills.Length; i++)
      {
         skills[i] = previousSkills[i];
      }
   }
   
   public void InteractApplyButton()
   {
      for (int i = 0; i < skills.Length; i++)   //스킬 12개 이하면 리턴
      {
         if (!skills[i])
         {
            applyButton.interactable = false;
            return;
         }
      }
      applyButton.interactable = true;
   }
}
