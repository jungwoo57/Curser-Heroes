using UnityEngine;

public class PulseEffect : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 offset = new Vector3(0f, -3f, 0f);  // 마우스보다 아래쪽

    private void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        transform.position = mouseWorldPos + offset;  // 항상 offset만큼 아래에 위치
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // "Projectile" 레이어의 인덱스를 가져옴
        int projectileLayer = LayerMask.NameToLayer("Projectile");

        if (other.gameObject.layer == projectileLayer)
        {
            Debug.Log("[빛의 파동] 적 투사체 제거됨 (Projectile 레이어)");
            Destroy(other.gameObject);
        }
    }
}