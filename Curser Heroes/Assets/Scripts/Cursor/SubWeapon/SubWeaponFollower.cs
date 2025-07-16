using UnityEngine;

public class SubWeaponFollower : MonoBehaviour
{
    private Transform target;
    private SpriteRenderer spriteRenderer;

    public float followDistance = 1.5f;
    public float rotationSpeed = 180f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetSprite(Sprite sprite)
    {
        if (spriteRenderer != null && sprite != null)
            spriteRenderer.sprite = sprite;
    }

    private SubWeaponData weaponData;

    public void Init(SubWeaponData data)
    {
        weaponData = data;

        // 스프라이트 자동 설정
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (weaponData != null && weaponData.weaponSprite != null)
        {
            spriteRenderer.sprite = weaponData.weaponSprite;
        }
    }



    void Update()
    {
        //  커서 위치 기준으로 회전 궤도 계산
        Vector3 cursorWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorWorldPos.z = 0f;

        float angle = Time.time * rotationSpeed;
        Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * followDistance;

        transform.position = cursorWorldPos + offset;

        if (target != null)
        {
            Vector3 dir = target.position - transform.position;
            float zRot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, zRot);
        }
    }
}
