using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text skillNameText;

    public void SetSkill(SkillManager.SkillInstance skillInstance)
    {
        icon.sprite = skillInstance.skill.icon;
        skillNameText.text = $"{skillInstance.skill.skillName} Lv.{skillInstance.level}";
    }
}
