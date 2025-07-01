using UnityEngine;
using UnityEngine.UI;
public class SkillPanelUI : MonoBehaviour
{
   public Button exitButton;

   public void OnClickExit()
   {
      gameObject.SetActive(false);
   }
}
