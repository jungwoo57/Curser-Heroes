using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text skillNameText;
    public TMP_Text descriptionText;

    public void SetSkill(SkillManager.SkillInstance skillInstance)
    {
        icon.sprite = skillInstance.skill.icon;
        skillNameText.text = $"{skillInstance.skill.skillName} Lv.{skillInstance.level}";
        descriptionText.text = skillInstance.skill.description;
    }
}
