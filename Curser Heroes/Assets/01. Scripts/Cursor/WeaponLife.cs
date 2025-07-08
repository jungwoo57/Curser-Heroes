using UnityEngine;

public class WeaponLife : MonoBehaviour
{
    public WeaponData currentWeapon;       //현재 장착된 무기 데이터 
    public int currentLives { get; private set; }   //남은 목숨 확인(외부에서 읽기 전용)

    public void Init(WeaponData weaponData)       //무기 장착시 초기화
    {
        currentWeapon = weaponData;               //무기 정보 저장
        currentLives = currentWeapon.maxLives;     // 목숨 초기화
    }

    public void TakeLifeDamage()            //목숨이 감소했을 때 함수
    {
        currentLives--;                    //목숨 -1
        UIManager.Instance.battleUI.TakeDamage();
        Debug.Log($"{currentWeapon.weaponName} 목숨 -1 → 현재: {currentLives}");
        if (currentLives <= 0)                    
            OnOutOfLives();                    //목숨이 0 이하가 되면 파괴 처리
    }

    [ContextMenu("사망 애니메이션")]
    private void OnOutOfLives()           //목숨이 0일 때 호출되는 함수
    {
        Debug.Log($"{currentWeapon.weaponName} 파괴됨 (목숨 0)");  //  목숨이 0이되면 디버그로그로 확인  
        WeaponManager.Instance.isDie = true;
        WeaponManager.Instance.StartCoroutine("DieAnimation");
    }

    
}
