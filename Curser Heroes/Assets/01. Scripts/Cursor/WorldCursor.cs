using UnityEngine;

public class WorldCursor : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        Cursor.visible = false;  // 기본 시스템 커서 숨기기
    }

    void Update()
    {
        if (WeaponManager.Instance.isDie) return;
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);
        transform.position = new Vector3(worldPos.x, worldPos.y, 0f); 
    }
}
