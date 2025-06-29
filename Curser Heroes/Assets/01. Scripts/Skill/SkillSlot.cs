using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSlot : MonoBehaviour
{
    public Image icon;
    public TMP_Text skillNameText;
    public TMP_Text descriptionText;

    private Action onClick;

    public void Set(SkillData skillData, Action onClickCallback)
    {
        icon.sprite = skillData.icon;
        skillNameText.text = $"{skillData.skillName} Lv.1";
        descriptionText.text = skillData.description;

        onClick = onClickCallback;

        var btn = GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => onClick?.Invoke());
    }

    public void Set(SkillManager.SkillInstance skillInstance, Action onClickCallback)
    {
        icon.sprite = skillInstance.skill.icon;
        skillNameText.text = $"{skillInstance.skill.skillName} Lv.{skillInstance.level}";
        descriptionText.text = skillInstance.skill.description;

        onClick = onClickCallback;

        var btn = GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => onClick?.Invoke());
    }
}