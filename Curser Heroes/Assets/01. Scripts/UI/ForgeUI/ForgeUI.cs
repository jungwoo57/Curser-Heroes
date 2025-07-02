using System;
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
    public TextMeshProUGUI weaponHP;
    public TextMeshProUGUI weaponAtk;
    
    public Button reinforceButton;
    public WeaponData selectWeapon;

    public void Init()
    {
        selectWeapon = GameManager.Instance.mainEquipWeapon;
        TextUpdate();
        ImageUpdate();
    }

    public void DisableReinforceButton()
    {
        reinforceButton.interactable = false;
    }

    public void OnClickReinforceButton()
    {
        if (GameManager.Instance.GetGold() >= selectWeapon.upgradeCost)
        {
            GameManager.Instance.AddGold(-selectWeapon.upgradeCost);
            selectWeapon.level++;
        }
        
        Debug.Log("무기강화");       //weapondata에 레벨이 존재해야 할 것 같음
    }

    public void TextUpdate()
    {
        weaponName.text = selectWeapon.weaponName;
        weaponDesc.text = selectWeapon.weaponDesc;
        weaponAtk.text = selectWeapon.baseDamage.ToString();
        weaponHP.text = selectWeapon.maxLives.ToString();
        hasGoldText.text = GameManager.Instance.GetGold().ToString();
        useGoldText.text = selectWeapon.upgradeCost.ToString();
    }

    public void ImageUpdate()
    {
        weaponImage.sprite = selectWeapon.weaponImage;
    }
}
