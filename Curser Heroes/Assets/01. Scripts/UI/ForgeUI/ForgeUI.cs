using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ForgeUI : MonoBehaviour
{
    public Image weaponImage;
    public Image WeaponTextInfo;
    public TextMeshProUGUI hasGoldText;
    public TextMeshProUGUI useGoldText;
    public Button reinforceButton;

    
    
    public void DisableReinforceButton()
    {
        reinforceButton.interactable = false;
    }

    public void OnClickReinforceButton()
    {
        Debug.Log("무기강화");       //weapondata에 레벨이 존재해야 할 것 같음
    }
}
