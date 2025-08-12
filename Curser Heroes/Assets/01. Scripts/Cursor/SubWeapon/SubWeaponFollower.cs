using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SubWeaponFollower : MonoBehaviour
{
    private Transform followTarget;
    private float currentAngle, angleOffset;
    private Coroutine flickerCoroutine;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public float followDistance = 1.5f;
    public float rotationSpeed = 360f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void Init(SubWeaponData data, float offset = 0f)
    {
        if (spriteRenderer != null && data.weaponSprite != null)
            spriteRenderer.sprite = data.weaponSprite;
        angleOffset = offset;
        currentAngle = offset;
    }

    public void SetMainWeapon(Transform target, float offset)
    {
        followTarget = target;
        angleOffset = offset;
        currentAngle = offset;
    }

    void Update()
    {
        if (followTarget == null) return;
        currentAngle = (currentAngle + rotationSpeed * Time.deltaTime) % 360f;
        float rad = currentAngle * Mathf.Deg2Rad;
        transform.position = followTarget.position
                           + new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f)
                             * followDistance;
    }

    // 충전 시작하면 깜빡이기
    public void StartCharging()
    {
        if (flickerCoroutine == null)
            flickerCoroutine = StartCoroutine(FlickerWhite());
    }

    // 깜빡 멈추고 원래 색으로
    public void StopCharging()
    {
        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
            flickerCoroutine = null;
        }
        spriteRenderer.color = originalColor;
    }

    // 완전 충전되면 빨간색으로
    public void SetCharged()
    {
        StopCharging();
        spriteRenderer.color = Color.red;
    }

    private IEnumerator FlickerWhite()
    {
        while (true)
        {
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
