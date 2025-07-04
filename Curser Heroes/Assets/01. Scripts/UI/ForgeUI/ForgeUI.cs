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

    [Header("무기 선택 이미지")] 
    public ForgeWeaponUI[] weaponUIs;
    
    [Header("버튼 목록")]
    public Button reinforceButton;
    public Button mainWeaponButton;
    public Button subWeaponButton;

    public bool isMain = true;
    
    public OwnedWeapon selectWeapon;
    public OwnedSubWeapon selectSubWeapon;
    
    private void OnEnable()
    {
        isMain = true;
        Init();
    }

    public void Init()
    {
        if (GameManager.Instance.mainEquipWeapon.data == null)     //장착 무기 없으면 1번 착용
        {
            Debug.Log("무기가없긴함");
            selectWeapon = GameManager.Instance.ownedWeapons[0];
            selectSubWeapon = GameManager.Instance.ownedSubWeapons[0];
        }
        else
        {
            selectWeapon = GameManager.Instance.mainEquipWeapon;
            selectSubWeapon = GameManager.Instance.subEquipWeapon;
        }

        UIUpdate();
        UpdateSelectUI();
    }
    
    public void OnClickReinforceButton()
    {
        if (GameManager.Instance.GetGold() >= selectWeapon.data.upgradeCost)
        {
            GameManager.Instance.UpgradeWeapon(selectWeapon.data);
            UIUpdate();
        }
        
        Debug.Log("무기강화");       //weapondata에 레벨이 존재해야 할 것 같음
    }

    public void UIUpdate()
    {
        if (isMain)
        {
            weaponName.text = selectWeapon.data.weaponName + "   (" + (selectWeapon.level + 1) + ")";
            weaponDesc.text = selectWeapon.data.weaponDesc;
            weaponAtk.text = ("공격력 : ") + selectWeapon.levelDamage.ToString();
            weaponHp.text = ("체력 : ") + selectWeapon.data.maxLives.ToString();
            hasGoldText.text = GameManager.Instance.GetGold().ToString();
            useGoldText.text = selectWeapon.data.upgradeCost.ToString();
            weaponImage.sprite = selectWeapon.data.weaponImage;
        }
        else
        {
            weaponName.text = selectSubWeapon.data.weaponName + "   (" + (selectWeapon.level + 1) + ")";
            weaponDesc.text = selectSubWeapon.data.weaponDesc;
            weaponAtk.text = ("공격력 : ") + selectSubWeapon.levelDamage.ToString();
           // weaponHp.text = ("체력 : ") + selectSubWeapon.data.maxLives.ToString();
            hasGoldText.text = GameManager.Instance.GetGold().ToString();
            useGoldText.text = selectWeapon.data.upgradeCost.ToString();
            weaponImage.sprite = selectSubWeapon.data.weaponImage;
        }
        if (GameManager.Instance.GetGold() < selectWeapon.data.upgradeCost)
        {
            reinforceButton.interactable = false;
        }
    }
    
    public void ClickExitButton()
    {
        gameObject.SetActive(false);
    }

    public void UpdateSelectUI()
    {
        if (isMain)
        {
            for (int i = 0; i < GameManager.Instance.allMainWeapons.Count; i++)
            {
                weaponUIs[i].UpdateUI(GameManager.Instance.allMainWeapons[i]);
            }
            for (int i = GameManager.Instance.allMainWeapons.Count; i < weaponUIs.Length; i++)
            {
                weaponUIs[i].UpdateUI();
            } 
        }
        else
        {
            for (int i = 0; i < GameManager.Instance.allSubWeapons.Count; i++)
            {
                weaponUIs[i].UpdateUI(GameManager.Instance.allSubWeapons[i]);
            }

            for (int i = GameManager.Instance.allSubWeapons.Count; i < weaponUIs.Length; i++)
            {
                weaponUIs[i].UpdateUI();
            } 
        }
    }
    public void ClickWeaponChangeButton()        //무기 변경
    {
        isMain = !isMain;
        if (isMain)
        {
            mainWeaponButton.interactable = false;
            subWeaponButton.interactable = true;
        }
        else
        {
            subWeaponButton.interactable = false;
            mainWeaponButton.interactable = true;
        }
        
        UIUpdate();
        UpdateSelectUI();
    }
}
