using UnityEngine;

public class ShieldRotator : MonoBehaviour
{
    private Transform center;
    private float speed;
    private float angleOffset;
    private float radius = 1.5f;
    private float angle;

    public void Initialize(Transform centerTarget, float rotateSpeed, float initialAngle)
    {
        center = centerTarget;
        speed = rotateSpeed;
        angle = initialAngle;
        angleOffset = initialAngle;
    }

    void Update()
    {
        angle += speed * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;
        transform.position = center.position + new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;
    }
}
