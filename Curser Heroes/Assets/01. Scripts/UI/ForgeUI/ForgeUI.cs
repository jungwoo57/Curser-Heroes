using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ForgeUI : MonoBehaviour
{
    public Image weaponImage;
    public Image WeaponTextInfo;
    
    [Header("텍스트 목록")]
    public TextMeshProUGUI hasGoldText;
    public TextMeshProUGUI useGoldText;
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI weaponDesc;
    public TextMeshProUGUI weaponHp;
    public TextMeshProUGUI weaponAtk;
    
    public Button reinforceButton;
    public OwnedWeapon selectWeapon;

    private void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        if (GameManager.Instance.mainEquipWeapon.data == null)     //장착 무기 없으면 1번 착용
        {
            Debug.Log("무기가없긴함");
            selectWeapon = GameManager.Instance.ownedWeapons[0];
        }
        else
        {
            selectWeapon = GameManager.Instance.mainEquipWeapon;
        }

        TextUpdate();
        ImageUpdate();
    }

    public void DisableReinforceButton()
    {
        reinforceButton.interactable = false;
    }

    public void OnClickReinforceButton()
    {
        if (GameManager.Instance.GetGold() >= selectWeapon.data.upgradeCost)
        {
            GameManager.Instance.UpgradeWeapon(selectWeapon.data);
            TextUpdate();
        }
        
        Debug.Log("무기강화");       //weapondata에 레벨이 존재해야 할 것 같음
    }

    public void TextUpdate()
    {
        weaponName.text = selectWeapon.data.weaponName + "  (" + (selectWeapon.level+1) +")";
        weaponDesc.text = selectWeapon.data.weaponDesc;
        weaponAtk.text = selectWeapon.levelDamage.ToString();
        weaponHp.text = selectWeapon.data.maxLives.ToString();
        hasGoldText.text = GameManager.Instance.GetGold().ToString();
        useGoldText.text = selectWeapon.data.upgradeCost.ToString();
    }

    public void ImageUpdate()
    {
        weaponImage.sprite = selectWeapon.data.weaponImage;
    }

    public void ClickExitButton()
    {
        gameObject.SetActive(false);
    }
}
