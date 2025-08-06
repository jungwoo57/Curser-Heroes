using UnityEngine;
using TMPro;
using Unity.Collections;
using UnityEngine.UI;
public class BarPartnerImageButton : MonoBehaviour
{
    [SerializeField]private BarPanel barPanel;
    //[SerializeField]private TextMeshProUGUI name;
    [SerializeField]public PartnerData partnerData;
    [SerializeField]public OwnedPartner ownedPartnerData;
    public Image partnerImage;
    public Button button;
    private bool isLocked;

    [SerializeField] private Color lockedColor;
    [SerializeField] private Color unlockedColor;
    private void OnEnable()
    {
        if (barPanel.selectData == partnerData)
        {
            button.interactable = false;
        }
    }

    public void UIUpdate()
    {
        CheckLock();
        partnerImage.sprite = partnerData.portraitSprite;
        if(isLocked) partnerImage.color = lockedColor;
        else partnerImage.color = unlockedColor;
    }

    public void SetData(PartnerData data)
    {
        partnerData = data;
        ownedPartnerData = GameManager.Instance.ownedPartners.Find(p
            => p.data.partnerName == partnerData.partnerName);
    }

    public void OnClick()
    {
        if (barPanel != null)
        {
            barPanel.selectData = partnerData;
            barPanel.selectPartner = ownedPartnerData;
            barPanel.UIUpdate();
            button.interactable = false;
        }
        
    }
    
    private void CheckLock()
    {
        //lock 인지 확인
        isLocked = GameManager.Instance.ownedPartners.Find(p
            => p.data.partnerName == partnerData.partnerName) == null;
    }
}
