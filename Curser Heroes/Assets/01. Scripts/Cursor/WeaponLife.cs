using UnityEngine;

public class WeaponLife : MonoBehaviour
{
    public WeaponData currentWeapon;       //현재 장착된 무기 데이터 
    public int maxLives { get; private set; }
    public int currentLives { get; private set; }   //남은 목숨 확인(외부에서 읽기 전용)
    
    public void Init(WeaponData weaponData)       //무기 장착시 초기화
    {
        currentWeapon = weaponData;               //무기 정보 저장
        maxLives = currentWeapon.maxLives;        // 원본 maxLives를 복사해서 사용
        currentLives = currentWeapon.maxLives;     // 목숨 초기화
        Debug.Log($"Init: {currentWeapon.weaponName} 목숨 {currentLives}");
    }

    public void TakeLifeDamage()
    {
        if (currentLives <= 0) return;

        currentLives--;
        UIManager.Instance.battleUI.TakeDamage();
        Debug.Log($"{currentWeapon.weaponName} 목숨 -1 → 현재: {currentLives}");
    }
    public void RecoverLife()
    {
        if (currentLives < maxLives)
        {
            currentLives++;
            UIManager.Instance.battleUI.Heal();
            Debug.Log($"[구원] 목숨 1 회복됨 → 현재: {currentLives}");
        }
    }
    public void IncreaseMaxLife(int amount)
    {
        maxLives += amount;
        currentLives = Mathf.Min(currentLives + amount, maxLives);  // 회복도 1회 같이 처리
        Debug.Log($"[거인] 최대 목숨 +{amount} → 현재: {currentLives}/{maxLives}");
    }

    [ContextMenu("사망 애니메이션")]
    private void OnOutOfLives()
    {
        Debug.Log($"OnOutOfLives 호출! 현재 목숨: {currentLives}");
        Debug.Log($"{currentWeapon.weaponName} 파괴됨 (목숨 0)");

        WeaponManager.Instance.isDie = true;
        WeaponManager.Instance.StartCoroutine("DieAnimation");
    }


}
