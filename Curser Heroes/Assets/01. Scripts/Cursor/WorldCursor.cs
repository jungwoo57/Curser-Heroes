using UnityEngine;

public class WorldCursor : MonoBehaviour
{
    [Header("Cursor Settings")]
    [Tooltip("클수록 마우스 이동에 더 민감하게 반응 (Lerp 사용 시)")]
    [Range(0.1f, 20f)]
    public float sensitivity = 5f;

    [Tooltip("클수록 더 부드럽게 따라옴 (SmoothDamp 사용 시)")]
    [Range(0.01f, 0.5f)]
    public float smoothing = 0.05f;

    private Camera cam;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        cam = Camera.main;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;  
        transform.position = GetMouseWorldPosition();
    }

    void Update()
    {

        if (WeaponManager.Instance != null && WeaponManager.Instance.isDie)
            return;


        Vector3 targetPos = GetMouseWorldPosition();
        


        //transform.position = Vector3.Lerp(
        //    transform.position,
        //   targetPos,
        //    sensitivity * Time.deltaTime
        //);
    
        
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            smoothing
        );
    }

   
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreen = Input.mousePosition;
        
        float zDistance = 0f - cam.transform.position.z;
        
        Vector3 screenPoint = new Vector3(mouseScreen.x, mouseScreen.y, zDistance);
        Vector3 worldPos = cam.ScreenToWorldPoint(screenPoint);
        worldPos.z = 0f;
        return worldPos;
    }
}
