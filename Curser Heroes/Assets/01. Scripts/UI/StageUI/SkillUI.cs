using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text skillNameText;
    public string CurrentSkillName => skillInstance?.skill.skillName;

    private SkillManager.SkillInstance skillInstance; // 현재 스킬 인스턴스 저장용

    public void SetSkill(SkillManager.SkillInstance instance)
    {
        skillInstance = instance; // 저장
        icon.sprite = skillInstance.skill.icon;
        skillNameText.text = $"{skillInstance.skill.skillName} Lv.{skillInstance.level}";
    }

    public void UpdateIcon(Sprite newIcon)
    {
        icon.sprite = newIcon;
    }
    public void SetEmpty()
    {
        icon.sprite = null;
        skillNameText.text = "";
    }
}