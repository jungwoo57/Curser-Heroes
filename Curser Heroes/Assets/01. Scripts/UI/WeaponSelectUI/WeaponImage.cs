using System;
using UnityEngine;
using UnityEngine.UI;
public class WeaponImage : MonoBehaviour
{
    public Image weaponSprite;
    public Image bookmarkImage; // 즐겨찾기 체크모양
    public bool isBookmark;       // 북마크 체크// 이미지에 추후 합치기
    public OwnedWeapon data;
    public OwnedSubWeapon subData;
    public OwnedPartner partnerData;
    public int num;
    public Outline outline;
    
    public static event Action OnWeaponPanelUpdate;
    public static event Action<WeaponImage> checkSelect;
    private void OnEnable()
    {
        EquipMainWeaponUI.OnBookMark += ChangeBookMark;
        EquipSubWeaponUI.OnBookMark += ChangeBookMark;
        EquipPartnerUI.OnBookMark += ChangeBookMark;
        checkSelect += CheckSelected;
    }

    private void OnDisable()
    {
        EquipMainWeaponUI.OnBookMark -= ChangeBookMark;
        EquipSubWeaponUI.OnBookMark -= ChangeBookMark;
        EquipPartnerUI.OnBookMark -= ChangeBookMark;
        checkSelect -= CheckSelected;
    }

    public void WeaponUpdate(OwnedWeapon recieveData) // 매게변수 받아야 할 확률 높음
    {
        data = recieveData;
        if (recieveData == GameManager.Instance.mainEquipWeapon)
        {
            outline.enabled = true;
        }
        else
        {
            outline.enabled = false;
        }
        if (data.bookMark)
        {
            bookmarkImage.gameObject.SetActive(true);
        }
        else
        {
            bookmarkImage.gameObject.SetActive(false);
        }
        subData = null;
        partnerData = null;
        num = 0;
        this.gameObject.SetActive(true);
        if (recieveData.data.weaponImage != null)
        {
            weaponSprite.sprite = data.data.weaponImage;
        }
        else
        {
            Debug.Log("이미지 없음");
        }
        //if(isBookMark) { 북마크 색상 변경(빨간색)}
        //else{ 북마크 색상 변경 (흰색)}
    }
    
    public void WeaponUpdate(OwnedSubWeapon recieveData) // 매게변수 받아야 할 확률 높음
    {
        subData = recieveData;
        if (recieveData == GameManager.Instance.subEquipWeapon)
        {
            outline.enabled = true;
        }
        else
        {
            outline.enabled = false;
        }
        subData = recieveData;
        data = null;
        partnerData = null;
        if (subData.bookMark)
        {
            bookmarkImage.gameObject.SetActive(true);
        }
        else
        {
            bookmarkImage.gameObject.SetActive(false);
        }
        num = 1;
        this.gameObject.SetActive(true);
        if (recieveData.data.weaponSprite != null)
        {
            weaponSprite.sprite = subData.data.weaponSprite;
        }
        else
        {
            Debug.Log("이미지 없음");
        }
        //if(isBookMark) { 북마크 색상 변경(빨간색)}
        //else{ 북마크 색상 변경 (흰색)}
    }

    public void WeaponUpdate(OwnedPartner recieveData) // 매게변수 받아야 할 확률 높음
    {
        partnerData = recieveData;
        if (partnerData == GameManager.Instance.equipPartner)
        {
            outline.enabled = true;
        }
        else
        {
            outline.enabled = false;
        }
        
        if (partnerData.bookMark)
        {
            bookmarkImage.gameObject.SetActive(true);
        }
        else
        {
            bookmarkImage.gameObject.SetActive(false);
        }
        subData = null;
        data = null;
        num = 2;
        this.gameObject.SetActive(true);
        if (recieveData.data.portraitSprite != null)
        {
            weaponSprite.sprite = partnerData.data.portraitSprite;
        }
        else
        {
            Debug.Log("이미지 없음");
        }
    }

    public void ChangeBookMark(OwnedWeapon weapon)
    {
        if (data == null) return;
        if (data == weapon)
        {
            bookmarkImage.gameObject.SetActive(data.bookMark);
        }
    }
    
    public void ChangeBookMark(OwnedSubWeapon weapon)
    {
        if (subData == null) return;
        if (subData == weapon)
        {
            bookmarkImage.gameObject.SetActive(subData.bookMark);
        }
    }
    
    public void ChangeBookMark(OwnedPartner weapon)
    {
        if (partnerData == null) return;
        if (partnerData == weapon)
        {
            bookmarkImage.gameObject.SetActive(partnerData.bookMark);
        }
    }
    
    public void OnClickWeaponButton()
    {
        switch (num)
        {
            case 0 : 
                GameManager.Instance.EquipWeapon(data);
                OnWeaponPanelUpdate?.Invoke();        //우선 메인 무기만 변경 추후 변경
                checkSelect?.Invoke(this);
                break;  // 메인무기
            case 1 : 
                GameManager.Instance.EquipWeapon(subData);  // 보조 무기 변경
                OnWeaponPanelUpdate?.Invoke();      
                checkSelect?.Invoke(this);
                break;  // 서브 무기
            case 2 : 
                GameManager.Instance.EquipPartner(partnerData);  // 동료 변경
                OnWeaponPanelUpdate?.Invoke();      
                checkSelect?.Invoke(this);
                break;  // 동료
            default: return; break;
        }
    }

    public void CheckSelected(WeaponImage select)
    {
        if (select == this)
        {
            outline.enabled = true;
        }
        else
        {
            outline.enabled = false;
        }
    }
}
