using System.Collections.Generic;
using UnityEngine;

public class WeaponScroll : MonoBehaviour
{
    public List<WeaponImage> hasWeapons; // 매니저에 있는 무기 리스트 불러오기
    public List<WeaponImage> showWeapons = new List<WeaponImage>(); // 스크롤에서 보여줄 무기들
    public int hasWeaponCounts;
    public Transform content;   // content아래 생성 하기위해서 부모 설정
    
    public void UpdateScroll()
    { 
        // hasWeapons = GameManager.Instance.hasWeapon  매니저에 있는 무기리스트 가져오기
        // While(showWeapons.Count < hasWeapons.Count)  갯수 부족 할 시 동적생성
        // { 
        //      GameObject obj = Instantiate(weaponImagePrefab, content);
        //      WeaponImage weaponImage = obj.GetComponent<WeaponImage>();
        //      showWeapons.Add(weaponImage);
        // }
        // for(int i = 0 ; i < hasWeaponCounts; i++)
        // {
        //      hasWeapons[i].WeaponUpdate();          // WeaponImage 업데이트
        // }
        // for(int i = hasWeapons.Count; i < showWeapons.Count; i++)
        // {
        //      showWeapons[i].setActive(false);       // 남은 부분 끄기
        // }                                                
    }
}
