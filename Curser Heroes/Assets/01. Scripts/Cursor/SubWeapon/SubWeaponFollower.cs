using UnityEngine;

public class SubWeaponFollower : MonoBehaviour
{
    [Header("참조")]
    public Transform cursorTransform;       // 커서(주무기)
    public Transform directionIndicator;    // 시각적 방향 표시

    [Header("설정")]
    public float radius = 1.5f;             // 커서를 중심으로 한 거리

    void Update()
    {
        if (cursorTransform == null) return;

        // 1. 마우스 위치 계산
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        // 2. 커서 → 마우스 방향 계산
        Vector2 direction = (mouseWorldPos - cursorTransform.position).normalized;

 
      
    }
}
