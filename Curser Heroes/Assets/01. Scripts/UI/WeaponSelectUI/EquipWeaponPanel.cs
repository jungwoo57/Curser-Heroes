using UnityEngine;

public class EquipmWeaponPanel : MonoBehaviour
{
    public EquipMainWeaponUI equipMainWeaponUi; // 0 주무기 1 보조 무기 2 파트너
    public EquipSubWeaponUI equipSubWeaponUI;
    public EquipPartnerUI equipPartnerUI;
    
    private void OnEnable()
    {
        UpdatePanel();
        WeaponImage.OnWeaponPanelUpdate += UpdatePanel;
    }

    private void OnDisable()
    {
        WeaponImage.OnWeaponPanelUpdate -= UpdatePanel;
    }

    
    public void UpdatePanel()
    {
        if (GameManager.Instance.mainEquipWeapon.data != null)
        {
            equipMainWeaponUi.UpdateUI(GameManager.Instance.mainEquipWeapon); // 주무기 업데이트 우선 제작
        }

        if (GameManager.Instance.subEquipWeapon.data != null)
        {
            equipSubWeaponUI.UpdateUI(GameManager.Instance.subEquipWeapon);
        }

        if (GameManager.Instance.equipPartner.data != null)
        {
            equipPartnerUI.UpdateUI(GameManager.Instance.equipPartner);
        }
    }
}
