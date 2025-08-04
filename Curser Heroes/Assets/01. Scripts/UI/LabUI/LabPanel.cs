using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class LabPanel : MonoBehaviour
{
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescription;
    public TextMeshProUGUI skillEffect;
    public SkillData selectSkill;
    public LabSkillImage[] skillButton;
    public Button unlockButton;
    public TextMeshProUGUI hasJewelText;
    public TextMeshProUGUI useJewelText;
    [SerializeField]private ScrollRect scrollRect;
    [SerializeField]private TutorialUI tutorialUI;
    [SerializeField] private VideoPlayer skillPlayer;
    [SerializeField] private RawImage skillAnimImage;
    private void OnEnable()
    {
        tutorialUI.gameObject.SetActive(false);
        if (!GameManager.Instance.useLab)
        {
            tutorialUI.gameObject.SetActive(true);
        }
        scrollRect.verticalNormalizedPosition = 1.0f;
        selectSkill = null;
        skillName.text = "스킬 이름";
        skillDescription.text = "스킬 설명";
        skillEffect.text = "스킬 효과";
        hasJewelText.text = GameManager.Instance.GetJewel().ToString();
        useJewelText.text = "";
        UpdateSkillScroll(SkillType.All);
        unlockButton.interactable = false;
        skillPlayer.clip = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClickExitButton();
        }
    }

    public void ClickTypeButton(int num)
    {
        SkillType type = (SkillType)num;
        UpdateSkillScroll(type);
    }
    
    public void UpdateSkillInfo()
    {
        if (!selectSkill) return;
        skillName.text = selectSkill.skillName;
        skillDescription.text = selectSkill.description;
        hasJewelText.text = GameManager.Instance.GetJewel().ToString();
        useJewelText.text = selectSkill.unlockCost.ToString();
        if(selectSkill.animClip != null)
        {
            skillAnimImage.gameObject.SetActive(true);
            skillPlayer.clip = selectSkill.animClip;
        }
        else
        {
            skillAnimImage.gameObject.SetActive(false);
        }
    }

    public void UpdateSkillScroll(SkillType type)
    {
       // int skillindex = 0;
        for (int i = 0; i < skillButton.Length; i++)  //이미지 초기화
        {
           // skillButton[i].image.gameObject.SetActive(false);
           skillButton[i].gameObject.SetActive(false);
        }
        
        for (int i = 0; i < GameManager.Instance.allSkills.Count; i++)  // 타입별로 정렬
        {
            if (GameManager.Instance.allSkills[i]&&GameManager.Instance.allSkills[i].type==type || type==SkillType.All)
            {
                //skillButton[i].image.gameObject.SetActive(true);
                skillButton[i].gameObject.SetActive(true);
                //skillButton[i].image.sprite = GameManager.Instance.allSkills[i].icon;
                skillButton[i].Init(GameManager.Instance.allSkills[i]);
                if (GameManager.Instance.HasSkills.Find(n => n.skillName == GameManager.Instance.allSkills[i].skillName))
                {
                    skillButton[i].backGroundImage.color = new Color(1f, 1f, 1f, 1.0f);
                    skillButton[i].skillImage.color = new Color(1f, 1f, 1f, 1.0f);
                }
                else
                {
                    skillButton[i].backGroundImage.color =  new Color(1f, 1f, 1f, 0.2f);;
                    skillButton[i].skillImage.color =  new Color(1f, 1f, 1f, 0.2f);
                    //skillButton[i].color = new Color(1f, 1f, 1f, 0.2f);
                }
                //skillindex++;
            }
        }
    }
    
    

    public void UpdateSkillImage()
    {
      
    }
    public void ClickSkillButton(int skillIndex)
    {
        selectSkill = GameManager.Instance.allSkills[skillIndex];
        UpdateSkillInfo();
        for (int i = 0; i < skillButton.Length; i++)
        {
            Button btns = skillButton[i].GetComponent<Button>();
            btns.interactable = true;
        }
        Button skillbtn = skillButton[skillIndex].GetComponent<Button>();
        skillbtn.interactable = false;//추가코드
        if(GameManager.Instance.HasSkills.Find(n => n.skillName == selectSkill.skillName)
           || GameManager.Instance.GetJewel() <= selectSkill.unlockCost)
        {
            unlockButton.interactable = false;
        }
        else
        {
            unlockButton.interactable = true;
        }
    }

    public void UnlockSkill()
    {
        if (GameManager.Instance.HasSkills.Find(n => n.skillName == selectSkill.skillName))
        {
            Debug.Log("해당스킬이 있습니다");
        }
        else
        {
            GameManager.Instance.UnlockSkill(selectSkill);
            unlockButton.interactable = false;
            for (int i = 0; i < GameManager.Instance.allSkills.Count; i++)
            {
                if (GameManager.Instance.HasSkills.Find(n =>
                        n.skillName == GameManager.Instance.allSkills[i].skillName))
                {
                    //skillButton[i].image.color = new Color(1f, 1f, 1f, 1.0f);
                    skillButton[i].backGroundImage.color = new Color(1f, 1f, 1f, 1.0f);
                    skillButton[i].skillImage.color = new Color(1f, 1f, 1f, 1.0f);
                }
                else
                {
                    skillButton[i].backGroundImage.color =  new Color(1f, 1f, 1f, 0.2f);;
                    skillButton[i].skillImage.color =  new Color(1f, 1f, 1f, 0.2f);
                    //skillButton[i].image.color = new Color(1f, 1f, 1f, 0.2f);
                }
            }
        }
    }

    public void ClickExitButton()
    {
        gameObject.SetActive(false);
    }
}
