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

        int nextLevel = Mathf.Clamp(ownedLevel + 1, 1, skillData.maxLevel);
        skillNameText.text = $"{skillData.skillName} Lv.{nextLevel}";

        var nextLevelInfo = skillData.levelDataList[Mathf.Clamp(nextLevel - 1, 0, skillData.levelDataList.Count - 1)];

        if (ownedLevel == 0)
        {
            descriptionText.text = nextLevelInfo.description.TrimEnd();
        }
        else
        {
            var currentLevelInfo = skillData.levelDataList[Mathf.Clamp(ownedLevel - 1, 0, skillData.levelDataList.Count - 1)];
            string desc = "";

            // 데미지
            if (currentLevelInfo.damage != nextLevelInfo.damage)
                desc += $"▶ 데미지: {currentLevelInfo.damage} → {nextLevelInfo.damage}\n";

            // 범위
            if (currentLevelInfo.sizeMultiplier != nextLevelInfo.sizeMultiplier)
                desc += $"▶ 범위: {currentLevelInfo.sizeMultiplier} → {nextLevelInfo.sizeMultiplier}\n";

            // count의 의미를 스킬 이름에 따라 다르게 표기
            if (currentLevelInfo.count != nextLevelInfo.count && nextLevelInfo.count > 0)
            {
                string countLabel = GetCountLabel(skillData.skillName);
                desc += $"▶ {countLabel}: {currentLevelInfo.count} → {nextLevelInfo.count}\n";
            }

            // 지속시간
            if (currentLevelInfo.duration != nextLevelInfo.duration)
                desc += $"▶ 지속시간: {currentLevelInfo.duration}초 → {nextLevelInfo.duration}초\n";

            // 쿨타임
            if (currentLevelInfo.cooldown != nextLevelInfo.cooldown)
                desc += $"▶ 쿨타임: {currentLevelInfo.cooldown}초 → {nextLevelInfo.cooldown}초\n";

            descriptionText.text = desc.TrimEnd();
        }

        onClick = onClickCallback;
        var btn = GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => onClick?.Invoke());
    }
    private string GetCountLabel(string skillName)
    {
        switch (skillName)
        {
            case "매직소드":
                return "검 추가";
            case "수호의 방패":
                return "방패 추가";
            case "약점 포착":
                return "필요횟수 감소";
            case "포식자":
                return "발동 확률";
            default:
                return "카운트";
        }
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