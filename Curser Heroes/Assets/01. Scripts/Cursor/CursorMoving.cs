using UnityEngine;

public class CursorMoving : MonoBehaviour
{
    [Tooltip("0~1")]
    [Range(0f, 1f)]
    public float cursorMoving = 0.5f;

    

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        transform.position = Vector3.zero;
    }

    void Update()
    {
        
        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = -cam.transform.position.z;
        Vector3 targetPos = cam.ScreenToWorldPoint(mouseScreen);
        targetPos.z = 0f;

        
        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            cursorMoving
        );

       
    }
}
