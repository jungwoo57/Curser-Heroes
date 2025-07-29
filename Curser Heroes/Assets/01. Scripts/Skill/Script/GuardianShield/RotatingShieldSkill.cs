using UnityEngine;

public class RotatingShieldSkill : MonoBehaviour
{
    private GameObject shieldPrefab;
    private Transform player;
    private float rotateSpeed = 100f;
    private float radius = 1.5f;

    public void Init(SkillManager.SkillInstance skillInstance, Transform playerTransform)
    {
        
        player = playerTransform;

        if (player == null)
        {
            return;
        }

        var levelData = skillInstance.skill.levelDataList[skillInstance.level - 1];

        int shieldCount = levelData.count;
        float angleStep = 360f / shieldCount;
        float scaleMultiplier = levelData.sizeMultiplier;

        for (int i = 0; i < shieldCount; i++)
        {
            float angle = i * angleStep;
            float rad = angle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            GameObject shield = Instantiate(shieldPrefab, transform.position + offset, rotation, transform);
            shield.transform.localScale *= scaleMultiplier;

            // ⚠️ 프리팹에 DestroyProjectileOnContact + Collider (isTrigger) 필요
        }
        Debug.Log($"[RotatingShieldSkill] Init 완료 - 방패 {shieldCount}개 배치됨");
    }

    void Update()
    {
        if (player != null)
        {
            transform.position = player.position;
            transform.Rotate(Vector3.forward, -rotateSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("[RotatingShieldSkill] player가 null입니다.");
        }
    }

    public void UpdateShields(SkillManager.SkillInstance skillInstance)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        Init(skillInstance, player);
    }
}