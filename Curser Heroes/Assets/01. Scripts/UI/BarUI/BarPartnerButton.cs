using TMPro;
using UnityEngine;

public class BarPartnerButton : MonoBehaviour
{
    [SerializeField]private BarPanel barPanel;
    [SerializeField]private TextMeshProUGUI name;
    [SerializeField]public PartnerData partnerData;
    [SerializeField]public OwnedPartner ownedPartnerData;
    private bool isLocked;
    public void UIUpdate()
    {
        CheckLock();
        if (isLocked)
        {
            name.text = partnerData.name;
        }
        else
        {
            if (ownedPartnerData.level > 0)
            {
                name.text = partnerData.name + ownedPartnerData.level;
            }
            else
            {
                name.text = partnerData.name;
            }
        }
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
        }
    }
    
    private void CheckLock()
    {
        //lock 인지 확인
        isLocked = GameManager.Instance.ownedPartners.Find(p
            => p.data.partnerName == partnerData.partnerName) == null;
    }
}
