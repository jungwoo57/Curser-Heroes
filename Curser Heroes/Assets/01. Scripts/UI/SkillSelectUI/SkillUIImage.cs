using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillUIImage : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Image skillImage;
    public Image imageShadow;
    public SkillData data;
    public StageSkillSelectUI stageSkillSelectUI;
    public Outline outline;
    public bool isSelected;
    
    private float pressTime;     //누르고 있는 시간
    //private bool isHolding = false;
    [SerializeField]private float holdTime = 0.5f;     //눌러야하는 시간
    private void Awake()
    {
        StageSkillSelectUI ui =GetComponentInParent<StageSkillSelectUI>();
        if (ui != null)
        {
            stageSkillSelectUI = ui;
        }
    }

    private void OnEnable()
    {
        /*if(outline != null)
            outline.enabled = false;*/
    }
    public void UpdateUI(SkillData skillData)
    {
            if (skillData == null) return;
            data = skillData;
            if (data.icon == null)
            {
                Debug.Log("데이터 없음");
                return;
            }
            
        
        skillImage.sprite = data.icon;
        Color imgColor = skillImage.color;
        imgColor.a = 1f;
        skillImage.color = imgColor;

        imageShadow.sprite = data.icon;
        Color shadowColor = imageShadow.color;
        shadowColor.a = 1f;
        imageShadow.color = shadowColor;

        isSelected = false;
        if (stageSkillSelectUI != null && outline != null)
        {
            for (int i = 0; i < stageSkillSelectUI.skills.Count; i++)
            {
                if (stageSkillSelectUI.skills[i].skillName == data.skillName)
                {
                    isSelected = false;
                }
            }

            if (outline.enabled)
            {
                Debug.Log("아웃라인나111옴");
            }

            outline.enabled = isSelected;
        }
    }

    public void CancleSelect()
    {
        data = null;

        Color imgColor = skillImage.color;
        imgColor.a = 0f;
        skillImage.color = imgColor;

        Color shadowColor = imageShadow.color;
        shadowColor.a = 0f;
        imageShadow.color = shadowColor;
        /*if (outline != null)
        {
            outline.enabled = false;
        }*/
    }

    public void OnClickSkillButton()
    {
        if (stageSkillSelectUI != null)
        {
            stageSkillSelectUI.stageSelectedSkillUI.SelectSkill(data);
        }
        else
        {
            Debug.Log("부모 오브젝트 못찾음");
        }
        //데이터 넘겨주기
    }
    
    public void OnPressSkillButton()
    {
        if (stageSkillSelectUI != null)
        {
            stageSkillSelectUI.skillInfoPanelUI.gameObject.SetActive(true);
            stageSkillSelectUI.skillInfcoCloseButton.gameObject.SetActive(true);
            stageSkillSelectUI.skillInfoPanelUI.UpdateUI(data);
        }
        else
        {
            Debug.Log("부모 오브젝트 못찾음");
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        pressTime = Time.time;
        //isHolding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //isHolding = false;
        float duration = Time.time - pressTime;

        if (duration > holdTime)
        {
            OnPressSkillButton();
        }
        else
        {
            OnClickSkillButton();
        }
    }
}
