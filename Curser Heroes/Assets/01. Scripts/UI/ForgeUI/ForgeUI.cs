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
    public TextMeshProUGUI hasJewelText;
    public TextMeshProUGUI useJewelText;
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
    public Button unlockButton;
   
    public bool isMain = true;
    public OwnedWeapon selectWeapon;
    public OwnedSubWeapon selectSubWeapon;
	public WeaponData selectData;
	public SubWeaponData selectSubData;
	public Image GoldImage;
	public Image JewelImage;
    
    private void OnEnable()
    {
        isMain = true;
        Init();
    }

    public void Init()
    {
        if (GameManager.Instance.mainEquipWeapon.data == null)     //장착 무기 없으면 1번 착용
        {
            selectWeapon = GameManager.Instance.ownedWeapons[0];
            selectData = GameManager.Instance.ownedWeapons[0].data;
        }
        else
        {
            selectWeapon = GameManager.Instance.mainEquipWeapon;
            selectData = GameManager.Instance.mainEquipWeapon.data;
        }

        if (GameManager.Instance.subEquipWeapon.data == null)
        {
            selectSubWeapon = GameManager.Instance.ownedSubWeapons[0];
            selectSubData = GameManager.Instance.ownedSubWeapons[0].data;
        }
        else
        {
            selectSubWeapon = GameManager.Instance.subEquipWeapon;
            selectSubData = GameManager.Instance.subEquipWeapon.data;
        }
        UIUpdate();
        UpdateSelectUI();
    }
    
    public void OnClickReinforceButton()
    {
        if (isMain)
        {
            if (GameManager.Instance.GetGold() >= selectWeapon.data.upgradeCost[selectWeapon.level])
            {
                GameManager.Instance.UpgradeWeapon(selectWeapon.data);
                UIUpdate();
            }
            Debug.Log("무기강화");       //we
        }
        else
        {
            if (GameManager.Instance.GetGold() >= selectSubWeapon.data.upgradeCost[selectSubWeapon.level])
            {
                GameManager.Instance.UpgradeWeapon(selectSubWeapon.data);
                UIUpdate();
            }
            Debug.Log("보조무기강화");    
        } //weapondata에 레벨이 존재해야 할 것 같음
    }

    public void OnClickUnlockWeapon()
    {
        if (isMain)/// 주무기 추가
        {
            if (GameManager.Instance.GetJewel() >= selectData.unlockCost)
            {
                GameManager.Instance.UnlockWeapon(selectData);
            }
        }
        else ///보조 무기 추가
        {
            if (GameManager.Instance.GetJewel() >= selectSubData.unlockCost)
            {
                GameManager.Instance.UnlockWeapon(selectSubData);
            }
        }
        unlockButton.gameObject.SetActive(false);
        reinforceButton.gameObject.SetActive(true);
        UpdateSelectUI();
        UIUpdate();
    }
    public void UIUpdate()
    {
        if (isMain)
        {
            weaponDesc.text = selectData.weaponDesc;
            weaponHp.text = ("체력 : ") + selectData.maxLives.ToString();
            hasGoldText.text = GameManager.Instance.GetGold().ToString();
            hasJewelText.text = GameManager.Instance.GetJewel().ToString();
            useJewelText.text = selectData.unlockCost.ToString();
            weaponImage.sprite = selectData.weaponImage;
            if (selectWeapon != null)
            {
                weaponAtk.text = ("공격력 : ") + selectWeapon.levelDamage.ToString();
                weaponName.text = selectData.weaponName + "   (" + (selectWeapon.level + 1) + ")";
                useGoldText.gameObject.SetActive(true);
                useJewelText.gameObject.SetActive(false);
                useGoldText.text = selectData.upgradeCost[selectWeapon.level].ToString();
                if (GameManager.Instance.GetGold() < selectData.upgradeCost[selectWeapon.level] || selectWeapon.level+1 >=10)
                {
                    reinforceButton.interactable = false;
                }
            }
            else
            {
                weaponAtk.text = ("공격력 : ") + selectData.baseDamage.ToString();
                weaponName.text = selectData.weaponName;
                useGoldText.gameObject.SetActive(false);
                useJewelText.gameObject.SetActive(true);
                if (GameManager.Instance.GetJewel() < selectData.unlockCost)
                {
                    unlockButton.interactable = false;
                }
            }
        }
            
        else
        {
            weaponDesc.text = selectSubData.weaponDesc;
            hasGoldText.text = GameManager.Instance.GetGold().ToString();
            useGoldText.text = selectSubData.upgradeCost.ToString();
            weaponImage.sprite = selectSubData.weaponImage;
            hasJewelText.text = GameManager.Instance.GetJewel().ToString();
            useJewelText.text = selectSubData.unlockCost.ToString();
            if (selectSubWeapon != null)
            {
                weaponName.text = selectSubData.weaponName + "   (" + (selectWeapon.level + 1) + ")";
                weaponAtk.text = ("공격력 : ") + selectSubWeapon.levelDamage.ToString();
                useGoldText.gameObject.SetActive(true);
                useJewelText.gameObject.SetActive(false);
                useGoldText.text = selectSubData.upgradeCost[selectSubWeapon.level].ToString();
                if (GameManager.Instance.GetGold() < selectSubData.upgradeCost[selectSubWeapon.level+1] ||selectSubWeapon.level+1 >=10)
                {
                    reinforceButton.interactable = false;
                }
            }
            else
            {
                weaponAtk.text = ("공격력 : ") + selectSubData.baseDamage.ToString();
                weaponName.text = selectSubData.weaponName;
                useGoldText.gameObject.SetActive(false);
                useJewelText.gameObject.SetActive(true);
                weaponAtk.text = ("공격력 : ") + selectSubData.baseDamage.ToString();
            }
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
