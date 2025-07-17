using UnityEngine.UI;
using UnityEngine;
using System;

public class EquipInfoUI : MonoBehaviour
{
    public Image mainWeaponImage;
    public Image subWeaponImage;
    public Image partnerImage;

    private void OnEnable()
    {
        WeaponSelectUI.OnEquipUIUpdate += EquipUIUpdate;
        if (GameManager.Instance.mainEquipWeapon.data)
        {
            mainWeaponImage.sprite = GameManager.Instance.mainEquipWeapon.data.weaponImage;
        }
        if (GameManager.Instance.subEquipWeapon.data)
        {
            subWeaponImage.sprite = GameManager.Instance.subEquipWeapon.data.weaponSprite;
        }

        if (GameManager.Instance.equipPartner.data)
        {
            partnerImage.sprite = GameManager.Instance.equipPartner.data.portraitSprite;
        }
        
    }
    
    private void OnDisable()
    {
        WeaponSelectUI.OnEquipUIUpdate -= EquipUIUpdate;
    }

    public void EquipUIUpdate()
    {
        if (GameManager.Instance.mainEquipWeapon.data != null)
        {
            mainWeaponImage.sprite = GameManager.Instance.mainEquipWeapon.data.weaponImage;
        }

        if (GameManager.Instance.subEquipWeapon.data != null)
        {
            subWeaponImage.sprite = GameManager.Instance.subEquipWeapon.data.weaponSprite;
        }

        if (GameManager.Instance.equipPartner.data != null)
        {
            partnerImage.sprite = GameManager.Instance.equipPartner.data.portraitSprite;
        }
        //파트너 이미지 추후 추가
    }
}
