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

    public void Set(SkillData skillData, int ownedLevel, Action onClickCallback)
    {
        icon.sprite = skillData.icon;

        // 다음 레벨 = 미보유 시 Lv.1, 보유 중이면 Lv.n+1
        int nextLevel = Mathf.Clamp(ownedLevel + 1, 1, skillData.maxLevel);
        skillNameText.text = $"{skillData.skillName} Lv.{nextLevel}";

        // 해당 레벨에 맞는 설명 가져오기
        var levelInfo = skillData.levelDataList[Mathf.Clamp(nextLevel - 1, 0, skillData.levelDataList.Count - 1)];
        descriptionText.text = levelInfo.description;

        onClick = onClickCallback;
        var btn = GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => onClick?.Invoke());
    }
    public void Set(SkillManager.SkillInstance skillInstance, Action onClickCallback)
    {
        var level = skillInstance.level;
        var data = skillInstance.skill;
        icon.sprite = data.icon;
        skillNameText.text = $"{data.skillName} Lv.{level}";

        // 레벨에 맞는 설명 생성
        var levelInfo = data.levelDataList[Mathf.Clamp(level - 1, 0, data.levelDataList.Count - 1)];
        descriptionText.text = $"피해: {levelInfo.damage} / 수: {levelInfo.count} / 크기: {levelInfo.sizeMultiplier}";

        onClick = onClickCallback;
        var btn = GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => onClick?.Invoke());
    }
}