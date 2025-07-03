
using UnityEngine;
using UnityEngine.UI;
public class ForgeWeaponUI : MonoBehaviour
{
    public WeaponData mainData;
    public OwnedWeapon hasData;
    public SubWeaponData subData;
    public ForgeUI forgeUI;
    public Image image;
    public Text levelText;
    public bool locked;
    

    public void CheckLock()   //소유하고 있으면 잠그기 아니면 해금
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
    
    public void UpdateUI(WeaponData weaponData)
    {
        mainData = weaponData;
        CheckLock();
        image.sprite = mainData.weaponImage;
        if (locked)
        {
            image.color = new Color(1f, 1f, 1f, 100/255f);
        }
        else
        {
            hasData = GameManager.Instance.ownedWeapons.Find(w => w.data.weaponName == mainData.weaponName);
            //levelText.text = hasData.level.ToString();
        }
    }

    public void OnClickButton()
    {
        if (!locked)
        {
            forgeUI.selectWeapon = hasData;
            forgeUI.UIUpdate();
        }
        //가지고 있으면 무기 변경 아니면 무기 해금 일단 무기 변경만
    }
    
}
