using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponScroll : MonoBehaviour
{
    public IReadOnlyList<OwnedWeapon> hasWeapons; // 매니저에 있는 무기 리스트 불러오기
    public IReadOnlyList<SubWeaponData> hasSubWeapons;
    public List<WeaponImage> showWeapons = new List<WeaponImage>(); // 스크롤에서 보여줄 무기들
    public int hasWeaponCounts;
    public Transform content;   // content아래 생성 하기위해서 부모 설정
    public ScrollRect scrollRect;
    public int scrollCount;     // 일정 갯 수 이하 스크롤 x
    public GameObject weaponImagePrefabs; // 더미 데이터

    public void UpdateScroll(string weaponType)
    {
        for (int i = 0; i < showWeapons.Count; i++)
        {
            showWeapons[i].gameObject.SetActive(false);
        }
        switch (weaponType)
        {
            case "main":
                hasWeapons = GameManager.Instance.ownedWeapons; //매니저에 있는 주무기리스트 가져오기
                hasWeaponCounts = GameManager.Instance.ownedWeapons.Count;
                if (hasWeapons.Count > scrollCount) // 아이템이 일정 갯수 이하이면 스크롤 안되게 하기
                {
                    scrollRect.vertical = true;
                }
                else
                {
                    scrollRect.vertical = false;
                }

                while (showWeapons.Count < hasWeapons.Count) //갯수 부족 할 시 동적생성
                {
                    GameObject obj = Instantiate(weaponImagePrefabs, content);
                    WeaponImage weaponImage = obj.GetComponent<WeaponImage>();
                    showWeapons.Add(weaponImage);
                }

                for (int i = 0; i < hasWeaponCounts; i++)
                {
                    showWeapons[i].WeaponUpdate(hasWeapons[i]); // WeaponImage 업데이트
                }

                for (int i = hasWeapons.Count; i < showWeapons.Count; i++)
                {
                    showWeapons[i].gameObject.SetActive(false); // 남은 부분 끄기
                }

                break;

            case "sub":
                hasSubWeapons = GameManager.Instance.hasSubWeapon; //매니저에 있는 서브리스트 가져오기
                hasWeaponCounts = GameManager.Instance.hasSubWeapon.Count;
                if (hasSubWeapons.Count > scrollCount) // 아이템이 일정 갯수 이하이면 스크롤 안되게 하기
                {
                    scrollRect.vertical = true;
                }
                else
                {
                    scrollRect.vertical = false;
                }

                while (showWeapons.Count < hasSubWeapons.Count) //갯수 부족 할 시 동적생성
                {
                    GameObject obj = Instantiate(weaponImagePrefabs, content);
                    WeaponImage weaponImage = obj.GetComponent<WeaponImage>();
                    showWeapons.Add(weaponImage);
                }

                for (int i = 0; i < hasWeaponCounts; i++)
                {
                    showWeapons[i].WeaponUpdate(hasSubWeapons[i]); // WeaponImage 업데이트
                }

                for (int i = hasWeapons.Count; i < showWeapons.Count; i++)
                {
                    showWeapons[i].gameObject.SetActive(false); // 남은 부분 끄기
                }

                break;

            case "partner":
                break;
        }
    }
}

