using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class EquipSubWeaponUI : MonoBehaviour
{
    [Header("구성요소")]
    public Image weaponImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI attackTypeText;
    public TextMeshProUGUI statusText;
    public Image bookMarkImage;
    private OwnedSubWeapon selectWeapon;
    public static event Action<OwnedSubWeapon> OnBookMark;
    public void UpdateUI(OwnedSubWeapon data)
    {
        if (data == null) return;
        selectWeapon = data;
        weaponImage.sprite = data.data.weaponSprite; //이미지 필요
        nameText.text = data.data.weaponName ; // equipdata 강화레벨 필요
        descriptionText.text = data.data.weaponDesc;
        //attackTypeText.text = data.effectType.ToString();             //공격타입 관련 데이터 필요
        statusText.text = "공격력 : " + data.levelDamage; // weapondata 데미지 업데이트 필요
        if (data.bookMark)
        {
            bookMarkImage.color = Color.red;
        }
        else
        {
            bookMarkImage.color = Color.gray;
        }
    }

    public void ClickBookMark()
    {
        selectWeapon.EnrollBookMark();
        OnBookMark?.Invoke(selectWeapon);
        UpdateUI(selectWeapon);
    }
}
