using System;
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
    [SerializeField] private TextMeshProUGUI costgoldText;
    
    [Header("버튼 모음")]
    [SerializeField]private Button reinforceButton;
    [SerializeField]private Button unLockButton;
    [SerializeField]private Button exitButton;
    
    [Header("이미지 모음")] 
    [SerializeField]private Image mainImage;
    [SerializeField]private Image goldImage;
    [SerializeField]private Image jewelImage;
    [SerializeField]private Image useGoldImage;
    [SerializeField]private Image useJewelImage;
    
    [SerializeField]private BarPartnerButton[] partnerButtons;
        
    public OwnedPartner selectPartner;
    public PartnerData selectData;
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
            partnerButtons[i].GetComponent<Button>().interactable = true;
        }//인터렉티블 켜기
        jewelImage.gameObject.SetActive(false);
        goldImage.gameObject.SetActive(false);
        useGoldImage.gameObject.SetActive(false);
        useJewelImage.gameObject.SetActive(false);
        if (selectPartner != null)
        {
            if (selectPartner.level == 0) // 레벨 1일떈 표시 x
            {
                nameText.text = selectPartner.data.partnerName;
            }
            else
            {
                nameText.text = selectPartner.data.partnerName + selectPartner.level.ToString();
            }
            reinforceButton.gameObject.SetActive(true);
            unLockButton.gameObject.SetActive(false);
            mainImage.sprite = selectPartner.data.portraitSprite;
            descText.text = selectPartner.data.desc;
            goldImage.gameObject.SetActive(true);
            useGoldImage.gameObject.SetActive(true);
            if (selectPartner.level < selectPartner.data.upgradeCost.Length)
            {
                costText.text = selectData.gaugeMax[selectPartner.level].ToString();
                hasgoldText.text = GameManager.Instance.GetGold().ToString();
                costgoldText.text = selectData.upgradeCost[selectPartner.level].ToString();    
            }
            else
            {
                costText.text = "최대 레벨";
                reinforceButton.gameObject.SetActive(false);
            }
        }
        else
        {
            nameText.text = selectData.partnerName;
            mainImage.sprite = selectData.portraitSprite;
            descText.text = selectData.desc;
            unLockButton.gameObject.SetActive(true);
            reinforceButton.gameObject.SetActive(false);
            jewelImage.gameObject.SetActive(true);
            useJewelImage.gameObject.SetActive(true);
            costText.text = selectData.gaugeMax[0].ToString();
            costgoldText.text = selectData.unlockCost.ToString();  
            hasgoldText.text = GameManager.Instance.GetJewel().ToString();
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
        GameManager.Instance.UpgradePartner(selectData);
        UIUpdate();
    }

    public void Unlock()
    {
        GameManager.Instance.UnlockPartner(selectData);
        selectPartner = GameManager.Instance.ownedPartners.Find(p
            => p.data.partnerName == selectData.partnerName);
        UIUpdate();
    }

}
