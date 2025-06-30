using System;
using UnityEngine;
using UnityEngine.UI;
public class WeaponImage : MonoBehaviour
{
    public Image weaponSprite;
    public Image bookmarkButton; // 즐겨찾기 버튼
    public bool isBookmark;       // 북마크 체크// 이미지에 추후 합치기
    public WeaponData data;
    public SubWeaponData subData;
    public int num;
    public static event Action OnWeaponPanelUpdate;
    
    public void WeaponUpdate(WeaponData recieveData) // 매게변수 받아야 할 확률 높음
    {
        data = recieveData;
        subData = null;
        num = 0;
        Debug.Log("업데이트" + data);
        this.gameObject.SetActive(true);
        if (recieveData.weaponImage != null)
        {
            weaponSprite.sprite = data.weaponImage;
        }
        else
        {
            Debug.Log("이미지 없음");
        }
        //if(isBookMark) { 북마크 색상 변경(빨간색)}
        //else{ 북마크 색상 변경 (흰색)}
    }
    
    public void WeaponUpdate(SubWeaponData recieveData) // 매게변수 받아야 할 확률 높음
    {
        subData = recieveData;
        data = null;
        num = 1;
        Debug.Log("업데이트" + subData);
        this.gameObject.SetActive(true);
        if (recieveData.weaponImage != null)
        {
            weaponSprite.sprite = subData.weaponImage;
        }
        else
        {
            Debug.Log("이미지 없음");
        }
        //if(isBookMark) { 북마크 색상 변경(빨간색)}
        //else{ 북마크 색상 변경 (흰색)}
    }

    public void OnClickWeaponButton()
    {
        switch (num)
        {
            case 0 : 
                GameManager.Instance.EquipWeapon(data);
                OnWeaponPanelUpdate?.Invoke();        //우선 메인 무기만 변경 추후 변경
                break;  // 메인무기
            case 1 : 
                GameManager.Instance.EquipWeapon(subData);  // 보조 무기 변경
                OnWeaponPanelUpdate?.Invoke();      
                break;  // 서브 무기
            case 2 : break;  // 동료
            default: return; break;
        }
    }
}
