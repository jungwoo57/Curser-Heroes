using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Skill/SkillData")]
public class SkillData : MonoBehaviour
{
    public string skillName;
    public int damage;
    public int cooltime;
    public Sprite icon;
    public int maxLevel;
    [TextArea] public string description;

    [System.Serializable]
    public class SkillInstance
    {
        public SkillData skill;
        public int level;

        public bool IsMaxed => level >= skill.maxLevel;
    }
}
