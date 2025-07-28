using UnityEngine;
using System.Collections;

public class CursorMoving : MonoBehaviour
{
    [Header("Movement Settings")]
    [Range(0, 1f)]
    public float cursorSpeed = 1f;

    private float originalSpeed;
    private bool isStunned;

    private void Awake()
    {
        originalSpeed = cursorSpeed;
        transform.position = Vector3.zero;
    }

    private void Update()
    {
        if (!isStunned)
            MouseMoving();
    }

    void MouseMoving()
    {
        Vector2 objectPos = transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z)
        );
        if (Vector2.Distance(objectPos, mousePos) > 1.0f)
            transform.position = Vector2.Lerp(objectPos, mousePos, cursorSpeed);
    }

    
    public void Stun(float duration)
    {
        if (isStunned) return;
        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;
        yield return new WaitForSeconds(duration);
        isStunned = false;
    }
}
