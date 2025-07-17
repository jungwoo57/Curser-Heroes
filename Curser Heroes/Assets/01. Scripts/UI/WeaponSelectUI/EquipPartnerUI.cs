using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipPartnerUI : MonoBehaviour
{
    [Header("구성요소")]
    public Image partnerImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    //public Image bookMarkImage;
    public OwnedPartner selectPartner;
   
    public void UpdateUI(OwnedPartner ownedPartner)
    {
        selectPartner = ownedPartner;
        partnerImage.sprite = ownedPartner.data.portraitSprite;
        nameText.text = ownedPartner.data.partnerName ; // equipdata 강화레벨 필요
        descriptionText.text = ownedPartner.data.desc;
        /*if (ownedPartner.bookMark)
        {
            bookMarkImage.color = Color.red;
        }
        else
        {
            bookMarkImage.color = Color.gray;
        }*/
    }

    public void ClickBookMarkButton()
    {
        selectPartner.EnrollBookMark();
        UpdateUI(selectPartner);
    }
}
