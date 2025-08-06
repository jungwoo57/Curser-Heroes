using UnityEngine;

public class WorldCursor : MonoBehaviour
{
    [Header("Cursor Settings")]
    [Tooltip("클수록 마우스 이동에 더 민감하게 반응")]
    [Range(0.1f, 10f)]
    public float sensitivity = 5f;

    [Tooltip("클수록 더 부드럽게 따라옴, 0 = 즉시 이동")]
    [Range(0f, 1f)]
    public float smoothing = 0.1f;

    private Camera cam;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        cam = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined; // 게임 창 안에서만 커서 이동
    }

    void Update()
    {
        if (WeaponManager.Instance != null && WeaponManager.Instance.isDie)
            return;

        
        Vector3 mouseScreen = Input.mousePosition;
        Vector3 worldPos = cam.ScreenToWorldPoint(mouseScreen);
        worldPos.z = 0f;

        //민감도 적용
        transform.position = Vector3.Lerp(transform.position, worldPos, sensitivity * Time.deltaTime);

        //부드러운 움직임
         transform.position = Vector3.SmoothDamp(
             transform.position,
             worldPos,
             ref velocity,
             smoothing,
             Mathf.Infinity,
             Time.deltaTime
         );
    }
}
