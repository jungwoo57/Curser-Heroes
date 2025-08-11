using System;
using UnityEngine;
using UnityEngine.UI;
public class SkillPanelUI : MonoBehaviour
{
   public Button exitButton;
   public StageSelectedSkillUI skillSelectedUI;
   public GameObject warningPanel;
   [SerializeField] private TutorialImageUI tutorialImageUI;
   
   private void OnEnable()
   {
      warningPanel.SetActive(false);
      if (!GameManager.Instance.useSkillList)
      {
         ClickHintButton();
         GameManager.Instance.useSkillList = true;
         GameManager.Instance.Save();
      }
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

    public void CloseWarningPanel()
    {
        warningPanel.SetActive(false);
    }

    public void Exit()
   {
      gameObject.SetActive(false);
   }
    
   public void ClickHintButton()
   {
      tutorialImageUI.gameObject.SetActive(true);
   }
}
