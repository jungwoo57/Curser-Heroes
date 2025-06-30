using UnityEngine;

public class EquipWeaponPanel : MonoBehaviour
{
    public EquipWeaponUI[] EquipWeaponUis; // 0 주무기 1 보조 무기 2 파트너
    
    
    private void OnEnable()
    {
        WeaponImage.OnWeaponPanelUpdate += UpdatePanel;
    }

    private void OnDisable()
    {
        WeaponImage.OnWeaponPanelUpdate -= UpdatePanel;
    }

    
    public void UpdatePanel(int index)
    {
        EquipWeaponUis[index].UpdateUI(GameManager.Instance.equipWeapons[index]); // 주무기 업데이트 우선 제작
    }
}
