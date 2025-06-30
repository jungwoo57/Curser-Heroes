using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WeaponImage : MonoBehaviour
{
    public Image weaponSprite;
    public Image bookmarkButton; // 즐겨찾기 버튼
    public bool isBookmark;       // 북마크 체크
    public Button selectButton;  // 이미지에 추후 합치기
    public WeaponData data;

    private void Awake()
    {
        //weaponSprite = GetComponentInChildren<SpriteRenderer>();
    }

    public void WeaponUpdate(WeaponData recieveData) // 매게변수 받아야 할 확률 높음
    {
        data = recieveData;
        Debug.Log("업데이트" + data);
        this.gameObject.SetActive(true);
        if (data.wepaonImage != null)
        {
            weaponSprite.sprite = data.wepaonImage;
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
        // 장착
    }
}
