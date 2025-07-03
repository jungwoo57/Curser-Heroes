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
        if (GameManager.Instance.mainEquipWeapon != null)
        {
            mainWeaponImage.sprite = GameManager.Instance.mainEquipWeapon.data.weaponImage;
        }

        if (GameManager.Instance.subEquipWeapon != null)
        {
            subWeaponImage.sprite = GameManager.Instance.subEquipWeapon.weaponImage;
        }
        //파트너 이미지 추후 추가
    }
}
