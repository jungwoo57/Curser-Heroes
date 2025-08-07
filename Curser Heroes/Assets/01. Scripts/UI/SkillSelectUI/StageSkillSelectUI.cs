using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StageSkillSelectUI : MonoBehaviour
{
    [SerializeField]public List<SkillData> hasSkills;
    [SerializeField]public List<SkillUIImage> showSkills = new List<SkillUIImage>();
    public SkillUIImage skillUIImage;
    public StageSelectedSkillUI stageSelectedSkillUI;
    public SkillInfoPanelUI skillInfoPanelUI;
    public Button skillInfcoCloseButton;
    public List<SkillData> skills = new List<SkillData>();
    
    private void Start()
    {
        hasSkills = GameManager.Instance.HasSkills;
        if (hasSkills != null)
        {
            UpdateUI();
        }
        else
        {
            Debug.Log("데이터 없음");
        }
        
        
    }

    private void OnEnable()
    {
        skillInfoPanelUI.gameObject.SetActive(false);
        skillInfcoCloseButton.gameObject.SetActive(false);
        hasSkills = GameManager.Instance.HasSkills;
        skills.Clear();
        for (int i = 0; i < stageSelectedSkillUI.skills.Length; i++)
        {
            skills.Add(null); // 리스트 초기화
        }
        
        UpdateUI();
    }

    public void UpdateUI()
    {
        
        while (showSkills.Count < hasSkills.Count)
        {
            SkillUIImage obj = Instantiate(skillUIImage ,this.transform);
            showSkills.Add(obj);
            Debug.Log("스킬 칸 동적 생성");
        }
        for (int i = 0; i < stageSelectedSkillUI.skills.Length; i++)
        {
            skills[i] = stageSelectedSkillUI.skills[i];
        }

        for (int i = 0; i < showSkills.Count; i++)         //이미지 업데이트
        {
            showSkills[i].gameObject.SetActive(true);
            showSkills[i].UpdateUI(hasSkills[i]);
        }
        
    }

    public void ClickSkillInfcoCloseButton()
    {
        skillInfoPanelUI.gameObject.SetActive(false);
        skillInfcoCloseButton.gameObject.SetActive(false);
    }
}
