
using UnityEngine;
using UnityEngine.UI;
public class ForgeWeaponUI : MonoBehaviour
{
    public WeaponData mainData;
    public OwnedWeapon hasData;
    public OwnedSubWeapon hasSubData;
    public SubWeaponData subData;
    public ForgeUI forgeUI;
    public Image image;
    public Text levelText;
    public bool locked;
    

    public void CheckMainLock()   //메인 무기 소유하고 있으면 잠그기 아니면 해금
    {
        var found = GameManager.Instance.ownedWeapons.Find(n => n.data.weaponName == mainData.weaponName);
        if(found == null)
        {
            locked = true;
        }
        else
        {
            locked = false;
        }
    }

    public void CheckSubLock()
    {
        var found = GameManager.Instance.ownedSubWeapons.Find(n => n.data.weaponName == subData.weaponName);
        if(found == null)
        {
            locked = true;
        }
        else
        {
            locked = false;
        }
    }
    
    public void UpdateUI(WeaponData weaponData)
    {
        mainData = weaponData;

        CheckMainLock();
        if (mainData != null)
        {
            image.sprite = mainData.weaponImage;
        }
        else
        {
            image.sprite = null;
        }
        if (locked)
        {
            image.color = new Color(1f, 1f, 1f, 100/255f);
        }
        else
        {
            image.color = new Color(1f, 1f, 1f, 1f);
            hasData = GameManager.Instance.ownedWeapons.Find(w => w.data.weaponName == mainData.weaponName);
            //levelText.text = hasData.level.ToString(); //레벨추가 예정
        }
    }

    public void UpdateUI(SubWeaponData weaponData)
    {
        subData = weaponData;
        CheckSubLock();
        if (mainData != null)
        {
            image.sprite = subData.weaponImage;
        }
        else
        {
            image.sprite = null;
        }
        if (locked)
        {
            image.color = new Color(1f, 1f, 1f, 100/255f);
        }
        else
        {
            image.color = new Color(1f, 1f, 1f, 1f);
            hasSubData = GameManager.Instance.ownedSubWeapons.Find(w => w.data.weaponName == subData.weaponName);
            //levelText.text = hasData.level.ToString();
        }
    }

    public void UpdateUI()
    {
        image.sprite = null;
        hasData = null;
        hasSubData = null;
        mainData = null;
        subData = null;
    }

    public void OnClickButton()
    {
        if (forgeUI.isMain)
        {
            if (!locked)
            {
                if (hasData == null) return;
                forgeUI.selectWeapon = hasData;
                forgeUI.selectData = hasData.data;
                forgeUI.reinforceButton.gameObject.SetActive(true);
                forgeUI.unlockButton.gameObject.SetActive(false);
                forgeUI.UIUpdate();
            }
            else
            {
                if (mainData == null) return;
                forgeUI.selectWeapon = null;
                forgeUI.selectData = mainData;
                forgeUI.reinforceButton.gameObject.SetActive(false);
                forgeUI.unlockButton.gameObject.SetActive(true);
                forgeUI.UIUpdate();
            }
        }
        else
        {
            if (!locked)
            {
                if (hasSubData == null) return;
                forgeUI.selectSubWeapon = hasSubData;
                forgeUI.selectSubData = hasSubData.data;
                forgeUI.reinforceButton.gameObject.SetActive(true);
                forgeUI.unlockButton.gameObject.SetActive(false);
                forgeUI.UIUpdate();
            }
            else
            {
                if (subData == null) return;
                forgeUI.selectSubWeapon = null;
                forgeUI.selectSubData = subData;
                forgeUI.reinforceButton.gameObject.SetActive(false);
                forgeUI.unlockButton.gameObject.SetActive(true);
                forgeUI.UIUpdate();
            }
        }
        //가지고 있으면 무기 변경 아니면 무기 해금 일단 무기 변경만
    }
    
}
