using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorMoving : MonoBehaviour
{
    public GameObject cursor;  
    [Range(0,1f)]
    public float cursorSpeed;   // 최고속도 1f
    private void Awake()
    {
        if (cursor == null)
        {
            cursor = this.gameObject; // 커서 할당
        }
        cursor.transform.position = Vector3.zero;
    }

    void Update()
    {
        MouseMoving();
    }

    void MouseMoving()
    {
        Vector2 objectPos = cursor.transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
            -Camera.main.transform.position.z));
        Vector2 moveDir = (mousePos - objectPos).normalized;
        if (Vector2.Distance(objectPos, mousePos) > 1.0f)
        {
           cursor.transform.position = Vector2.Lerp(cursor.transform.position,
                    mousePos,1f * cursorSpeed);
        }
    }

}
