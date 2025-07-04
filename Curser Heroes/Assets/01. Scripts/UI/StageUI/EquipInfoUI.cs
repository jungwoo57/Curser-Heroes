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
            subWeaponImage.sprite = GameManager.Instance.subEquipWeapon.data.weaponImage;
        }
        //파트너 이미지 추후 추가
    }
}
