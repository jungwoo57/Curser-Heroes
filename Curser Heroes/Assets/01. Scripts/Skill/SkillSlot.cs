using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class SkillSlot : MonoBehaviour
{
    public Image icon; // Icon 오브젝트
    public TMP_Text skillNameText;
    public TMP_Text descriptionText;

    private Action onClick;

    public void Set(SkillData skill, Action onClickCallback)
    {
        icon.sprite = skill.icon;
        skillNameText.text = skill.skillName + " Lv.1"; // or use actual level
        descriptionText.text = skill.description;

        onClick = onClickCallback;

        // Button은 현재 이 오브젝트에 붙어 있음
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() => onClick?.Invoke());
    }
}

