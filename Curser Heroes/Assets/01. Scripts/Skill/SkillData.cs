using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Skill/SkillData")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public int damage;
    public int cooltime;
    public Sprite icon;
    public int maxLevel;
    public string description;
}
