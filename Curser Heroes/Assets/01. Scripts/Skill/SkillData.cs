using System.Collections.Generic;
using UnityEngine;

public enum SkillType { Attack, Defense, Buff }

[CreateAssetMenu(fileName = "SkillData", menuName = "Skill/SkillData")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public Sprite icon;
    public SkillType type;
    public int maxLevel;
    public string description;

    public GameObject skillPrefab;

    public List<SkillLevelData> levelDataList; // 레벨 별 수치
}