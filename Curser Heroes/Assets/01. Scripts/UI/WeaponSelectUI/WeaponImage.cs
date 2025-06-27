using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WeaponImage : MonoBehaviour
{
    public Image weponImage;
    public Image bookmarkButton; // 즐겨찾기 버튼
    public bool isBookmark;       // 북마크 체크
    public Button selectButton;  // 이미지에 추후 합치기
    
    public void WeaponUpdate() // 매게변수 받아야 할 확률 높음
    {
        // weaponImage = 무기 이미지
        //if(isBookMark) { 북마크 색상 변경(빨간색)}
        //else{ 북마크 색상 변경 (흰색)}
    }

    public void OnClickWeaponButton()
    {
        // 장착
    }
}
