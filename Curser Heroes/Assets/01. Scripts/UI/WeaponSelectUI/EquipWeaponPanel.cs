using UnityEngine;

public class EquipmWeaponPanel : MonoBehaviour
{
    public EquipMainWeaponUI equipMainWeaponUi; // 0 주무기 1 보조 무기 2 파트너
    public EquipSubWeaponUI equipSubWeaponUI;
    
    private void OnEnable()
    {
        WeaponImage.OnWeaponPanelUpdate += UpdatePanel;
    }

    private void OnDisable()
    {
        WeaponImage.OnWeaponPanelUpdate -= UpdatePanel;
    }

    
    public void UpdatePanel()
    {
        equipMainWeaponUi.UpdateUI(GameManager.Instance.mainEquipWeapon); // 주무기 업데이트 우선 제작
        //equipSubWeaponUI.UpdateUI(GameManager.Instance.subEquipWeapon);
    }
}
