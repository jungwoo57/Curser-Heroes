using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BarPanel : MonoBehaviour
{
    [Header("텍스트 모음")]
    [SerializeField]private TextMeshProUGUI nameText;
    [SerializeField]private TextMeshProUGUI descText;
    [SerializeField]private TextMeshProUGUI costText;
    [SerializeField]private TextMeshProUGUI hasgoldText;
    [SerializeField]private TextMeshProUGUI hasJewelText;
    [SerializeField]private TextMeshProUGUI costgoldText;
    [SerializeField]private TextMeshProUGUI costjewelText;
    [SerializeField]private TextMeshProUGUI curgageText;
    [SerializeField] private TextMeshProUGUI aftergageText;
    [SerializeField]private TextMeshProUGUI reinforcementText;
    [SerializeField] private TextMeshProUGUI unlockText;
    
    
    [Header("버튼 모음")]
    [SerializeField]private Button reinforceButton;
    [SerializeField]private Button unLockButton;
    [SerializeField]private Button exitButton;
    [SerializeField] private Image doneButton;

    [Header("이미지 모음")] 
    [SerializeField]private Image mainImage;
    [SerializeField] private Image iconImage;
    [SerializeField]private Image goldImage;
    [SerializeField]private Image jewelImage;

    [Header("잠김 텍스트 컬러")] 
    [SerializeField] private Color cantTextColor;
    [SerializeField] private Color canTextColor;
    
    [SerializeField]private BarPartnerImageButton[] partnerButtons;
    public OwnedPartner selectPartner;
    public PartnerData selectData;

    [Header("강화 및 잠금 이펙트")] 
    [SerializeField] private GameObject[] upgradeDirections;
    [SerializeField] private GameObject unlockDirection;
    [SerializeField] private float effectDurationTime;
    private void OnEnable()
    {
        if (GameManager.Instance.equipPartner != null)
        {
            selectPartner = GameManager.Instance.equipPartner;
            selectData = GameManager.Instance.equipPartner.data;
        }
        else
        {
            selectPartner = null;
        }
        DataUpdate();
        UIUpdate();
    }

    public void UIUpdate()
    {
        selectPartner = GameManager.Instance.ownedPartners.Find(p
            => p.data.partnerName == selectData.partnerName);
        for (int i = 0; i < partnerButtons.Length; i++)
        {
            partnerButtons[i].UIUpdate();
        }//인터렉티블 켜기
        if (selectPartner != null)
        {
            if (selectPartner.level == 0) // 레벨 1일떈 표시 x
            {
                nameText.text = selectPartner.data.partnerName;
            }
            else
            {
                nameText.text = selectPartner.data.partnerName + " +" + selectPartner.level.ToString();
            }
            reinforceButton.gameObject.SetActive(true);
            reinforceButton.interactable = true;
            reinforcementText.color = canTextColor;
            unLockButton.gameObject.SetActive(false);
            unLockButton.interactable = true;
            doneButton.gameObject.SetActive(false);
            //mainImage.sprite = selectPartner.data.portraitSprite;
            iconImage.sprite = selectPartner.data.portraitSprite;
            descText.text = selectPartner.data.desc;
            goldImage.gameObject.SetActive(true);
            jewelImage.gameObject.SetActive(false);
            //useGoldImage.gameObject.SetActive(true);
            costgoldText.gameObject.SetActive(true);
            costjewelText.gameObject.SetActive(false);
            
            if (selectPartner.level < selectPartner.data.upgradeCost.Length)
            {
                costText.text = selectData.gaugeMax[selectPartner.level].ToString();
                hasgoldText.text = GameManager.Instance.GetGold().ToString();
                hasJewelText.text = GameManager.Instance.GetJewel().ToString();
                costgoldText.text = selectData.upgradeCost[selectPartner.level].ToString();
                costjewelText.text = selectData.upgradeCost[selectPartner.level].ToString();
                curgageText.text = selectData.gaugeMax[selectPartner.level].ToString();
                aftergageText.text = selectData.gaugeMax[selectPartner.level+1].ToString();
                if (selectPartner.data.upgradeCost[selectPartner.level] > GameManager.Instance.GetGold())
                {
                    reinforceButton.interactable = false;
                    reinforcementText.color = cantTextColor;
                }
            }
            else
            {
                costgoldText.text = "최대 레벨";
                aftergageText.text = "최대 레벨";
                curgageText.text = selectData.gaugeMax[selectPartner.level].ToString();
                reinforceButton.gameObject.SetActive(false);
                goldImage.gameObject.SetActive(false);
                doneButton.gameObject.SetActive(true);
            }
        }
        else
        {
            nameText.text = selectData.partnerName;
            //mainImage.sprite = selectData.portraitSprite;
            iconImage.sprite = selectData.portraitSprite;
            descText.text = selectData.desc;
            unlockText.color = canTextColor;
            unLockButton.gameObject.SetActive(true);
            reinforceButton.gameObject.SetActive(false);
            doneButton.gameObject.SetActive(false);
            jewelImage.gameObject.SetActive(true);
            goldImage.gameObject.SetActive(false);
            //useJewelImage.gameObject.SetActive(true);
            costgoldText.gameObject.SetActive(false);
            costjewelText.gameObject.SetActive(true);
            costText.text = selectData.gaugeMax[0].ToString();
            costjewelText.text = selectData.unlockCost.ToString();
            hasgoldText.text = GameManager.Instance.GetGold().ToString();
            hasJewelText.text = GameManager.Instance.GetJewel().ToString();
            curgageText.text = "미해금";
            aftergageText.text = selectData.gaugeMax[0].ToString();
            if (selectData.unlockCost > GameManager.Instance.GetJewel())
            {
                unLockButton.interactable = false;
                unlockText.color = cantTextColor;
            }
        }
    }
    

    private void DataUpdate()
    {
        for (int i = 0; i < GameManager.Instance.allPartners.Count; i++)
        {
            partnerButtons[i].SetData(GameManager.Instance.allPartners[i]);
            partnerButtons[i].UIUpdate();
        }

        for (int i = GameManager.Instance.allPartners.Count; i < partnerButtons.Length; i++)
        {
            partnerButtons[i].gameObject.SetActive(false);
        }
    }


    public void Reinforce()
    {
        float hasgold = GameManager.Instance.GetGold();
        if (hasgold >= selectData.upgradeCost[selectPartner.level])
        {
            GameManager.Instance.UpgradePartner(selectData);
            ReinforceEffect();
            UIUpdate();
        }
    }

    public void Unlock()
    {
        GameManager.Instance.UnlockPartner(selectData);
        selectPartner = GameManager.Instance.ownedPartners.Find(p
            => p.data.partnerName == selectData.partnerName);
        UnlockEffect();
        UIUpdate();
    }
    
    public void ReinforceEffect()
    {
        for (int i = 0; i < upgradeDirections.Length; i++)
        {
            if (!upgradeDirections[i].activeInHierarchy)
            {
                upgradeDirections[i].gameObject.SetActive(true);
                StartCoroutine(EffectTime(effectDurationTime, upgradeDirections[i]));
                return;
            }
        }
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

}
