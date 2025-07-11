using UnityEngine;

public class SubWeaponFollower : MonoBehaviour
{
    [Header("참조")]
    public Transform cursorTransform;       // 커서(주무기)
    public Transform directionIndicator;    // 시각적 방향 표시
    public SubWeaponManager subWeaponManager;  //서브웨펀 매니저 정보가져오기
    public SubWeaponType weaponType;
    
    [Header("설정")]
    public float radius = 1.5f;             // 커서를 중심으로 한 거리

    [Header("보조무기 UI")] 
    public GameObject ammoTypeUI;
    public GameObject manaTypeUI;
    public GameObject ChargeTypeUI;
    
    void Start()
    {
       // weaponType = subWeaponManager.equippedSubWeapon.weaponType;
    }
    void Update()
    {
        if (cursorTransform == null) return;

        // 1. 마우스 위치 계산
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        // 2. 커서 → 마우스 방향 계산
        Vector2 direction = (mouseWorldPos - cursorTransform.position).normalized;

 
      
    }

    public void UpdateUI()
    {
        
    }
}
