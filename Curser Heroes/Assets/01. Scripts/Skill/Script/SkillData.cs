using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public enum SkillType { Attack, Defense, Buff, All }

[System.Serializable]
[CreateAssetMenu(fileName = "SkillData", menuName = "Skill/SkillData")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public Sprite icon;
    public SkillType type;
    public int maxLevel;
    public string description;
    
    public GameObject skillPrefab;
    public VideoClip animClip;
    
    
    public List<SkillLevelData> levelDataList; // 레벨 별 수치
    public int unlockCost;
    public bool isDefaultSkill; // 기본 제공 스킬
}