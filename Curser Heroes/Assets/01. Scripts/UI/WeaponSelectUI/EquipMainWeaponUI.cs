using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class EquipMainWeaponUI : MonoBehaviour
{
   [Header("구성요소")]
   public Image weaponImage;
   public TextMeshProUGUI nameText;
   public TextMeshProUGUI descriptionText;
   public TextMeshProUGUI attackTypeText;
   public TextMeshProUGUI statusText;
   
   public void UpdateUI(OwnedWeapon ownedWeapon)
   {
      weaponImage.sprite = ownedWeapon.data.weaponImage;
      nameText.text = ownedWeapon.data.name ; // equipdata 강화레벨 필요
      descriptionText.text = ownedWeapon.data.weaponDesc;
      //attackTypeText.text = data.             공격타입 관련 데이터 필요
      statusText.text = "공격력 : " + ownedWeapon.levelDamage; // weapondata 데미지 업데이트 필요

   }
}
