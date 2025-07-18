using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
public class EquipPartnerUI : MonoBehaviour
{
    [Header("구성요소")]
    public Image partnerImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public Image bookMarkImage;
    public OwnedPartner selectPartner;
   
    public static event Action<OwnedPartner> OnBookMark;
    public void UpdateUI(OwnedPartner ownedPartner)
    {
        if (ownedPartner == null) return;
        selectPartner = ownedPartner;
        partnerImage.sprite = ownedPartner.data.portraitSprite;
        nameText.text = ownedPartner.data.partnerName ; // equipdata 강화레벨 필요
        descriptionText.text = ownedPartner.data.desc;
        if (ownedPartner.bookMark)
        {
            bookMarkImage.color = Color.red;
        }
        else
        {
            bookMarkImage.color = Color.gray;
        }
    }

    public void ClickBookMarkButton()
    {
        selectPartner.EnrollBookMark();
        OnBookMark?.Invoke(selectPartner);
        UpdateUI(selectPartner);
    }
}
