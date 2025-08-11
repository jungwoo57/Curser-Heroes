using System.Collections;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] private VideoPlayer skillPlayer;
    [SerializeField] private RawImage skillAnimImage;
    [SerializeField] private Image useJewelImage;
    
    [Header("이펙트 모음")] 
    [SerializeField] private GameObject unlockDirection;
    [SerializeField] private float effectDurationTime;

	[Header("스킬 텍스트")]
	[SerializeField] private TextMeshProUGUI skillTypeText;
    [SerializeField] private TextMeshProUGUI maxLevelText;
        
    [SerializeField] private TutorialImageUI tutorialImageUI;
    private void OnEnable()
    {
        scrollRect.verticalNormalizedPosition = 1.0f;
        selectSkill = null;
        skillName.text = "";
        skillDescription.text = "";
        skillEffect.text = "스킬 효과";
        hasJewelText.text = GameManager.Instance.GetJewel().ToString();
        useJewelText.text = "";
        skillTypeText.text = "";
        maxLevelText.text = "";
        
        UpdateSkillScroll(SkillType.All);
        unlockButton.interactable = false;
        useJewelImage.gameObject.SetActive(false);
        skillPlayer.clip = null;

        if (!GameManager.Instance.useLab)
        {
            ClickHintButton();
            GameManager.Instance.useLab = true;
            GameManager.Instance.Save();
        }
    }

    
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !tutorialImageUI.gameObject.activeInHierarchy)
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
        switch (selectSkill.type)
        {
            case SkillType.Attack:
                skillTypeText.text = "공격형";
                break;
            case SkillType.Defense:
                skillTypeText.text = "수비형";
                break;
            case SkillType.Buff:
                skillTypeText.text = "버프형";
                break;
        }
        
        maxLevelText.text = "최대 레벨 : " + (selectSkill.levelDataList.Count).ToString();
        hasJewelText.text = GameManager.Instance.GetJewel().ToString();
        useJewelText.text = selectSkill.unlockCost.ToString();
        useJewelImage.gameObject.SetActive(true);
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
    
    
    public void ClickSkillButton(int skillIndex)
    {
        selectSkill = GameManager.Instance.allSkills[skillIndex];
        UpdateSkillInfo();
        for (int i = 0; i < skillButton.Length; i++)
        {
            Button btns = skillButton[i].GetComponent<Button>();
            btns.interactable = true;
            skillButton[i].outline.enabled = false;
        }
        Button skillbtn = skillButton[skillIndex].GetComponent<Button>();
        skillbtn.interactable = false;//추가코드
        Outline outline = skillButton[skillIndex].GetComponent<Outline>();
        outline.enabled = true;
        if(GameManager.Instance.HasSkills.Find(n => n.skillName == selectSkill.skillName)
           || GameManager.Instance.GetJewel() <= selectSkill.unlockCost)
        {
            unlockButton.interactable = false;
            useJewelImage.gameObject.SetActive(false);
        }
        else
        {
            unlockButton.interactable = true;
            useJewelImage.gameObject.SetActive(true);
        }
        if(GameManager.Instance.HasSkills.Find(n => n.skillName == selectSkill.skillName))
        {
            unlockButton.interactable = false;
            useJewelImage.gameObject.SetActive(false);
            useJewelText.text = "이미 보유한 스킬입니다";
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
            if (GameManager.Instance.GetJewel() < selectSkill.unlockCost) return;
            GameManager.Instance.UnlockSkill(selectSkill);
            unlockButton.interactable = false;
            UnlockEffect();
            for (int i = 0; i < GameManager.Instance.allSkills.Count; i++)
            {
                if (GameManager.Instance.HasSkills.Find(n =>
                        n.skillName == GameManager.Instance.allSkills[i].skillName))
                {
                    //skillButton[i].image.color = new Color(1f, 1f, 1f, 1.0f);
                    skillButton[i].backGroundImage.color = new Color(1f, 1f, 1f, 1.0f);
                    skillButton[i].skillImage.color = new Color(1f, 1f, 1f, 1.0f);
                    useJewelImage.gameObject.SetActive(false);
                    useJewelText.text = "이미 보유한 스킬입니다";
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
    
    private void UnlockEffect()
    {
        unlockDirection.gameObject.SetActive(true);
        StartCoroutine(EffectTime(effectDurationTime, unlockDirection));
    }

    IEnumerator EffectTime(float durationTime, GameObject effect)
    {
        yield return new WaitForSeconds(durationTime);
        effect.SetActive(false);
    }
    
    public void ClickHintButton()
    {
        tutorialImageUI.gameObject.SetActive(true);
    }
}
