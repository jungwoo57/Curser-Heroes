using System;
using UnityEngine;
using UnityEngine.UI;
public class SkillPanelUI : MonoBehaviour
{
   public Button exitButton;
   public StageSelectedSkillUI skillSelectedUI;
   public GameObject warningPanel;

   private void OnEnable()
   {
      warningPanel.SetActive(false);
   }

   public void OnClickExit()
   {
      if (skillSelectedUI.isChange)
      {
         warningPanel.SetActive(true);
      }
      else
      {
         Exit();
      }
   }

   public void Exit()
   {
      gameObject.SetActive(false);
   }
}
