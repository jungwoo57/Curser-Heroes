using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillUIImage : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Image skillImage;
    public SkillData data;
    private StageSkillSelectUI stageSkillSelectUI;

    private float pressTime;     //누르고 있는 시간
    //private bool isHolding = false;
    [SerializeField]private float holdTime = 0.5f;     //눌러야하는 시간
    private void Awake()
    {
        stageSkillSelectUI = GetComponentInParent<StageSkillSelectUI>();
    }

    public void UpdateUI(SkillData skillData)
    {
            data = skillData;
            if (data.icon == null)
            {
                Debug.Log("데이터 없음");
                return;
            }
            skillImage.sprite = data.icon;
    }

    public void CancleSelect()
    {
        data = null;
        skillImage.sprite = null;
        return;
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
