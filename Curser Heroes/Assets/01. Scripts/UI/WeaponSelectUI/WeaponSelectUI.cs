using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WeaponSelectUI : MonoBehaviour
{
    [Header("UI 요소")]
    [SerializeField] private GameObject weaponSelectUI;
    [SerializeField] private Button mainWeaponSelectButton;
    [SerializeField] private Button subWeaponSelectButton;
    [SerializeField] private Button partnerSelectButton;
    [SerializeField] private GameObject selectScroll;             // 무기 선택 창
    [SerializeField] private GameObject weaponInfoScroll;        // 무기 정보 보여주는 판넬
    [SerializeField] private GameObject ExitButton;



    public void ChangeWeaponPanel(string buttonName)  // 무기 선택 화면 바꾸기
    {
        switch (buttonName)
        {
            case "main" :
                // 주무기 업데이트
                break;
            case "sub" : 
                // 보조무기 업데이트
                break;
            case "partner" :
                // 동료무기 업데이트
                break;
        }
    }
    
    public void ClickExitButton()
    {
        weaponSelectUI.SetActive(false);
    }
}
