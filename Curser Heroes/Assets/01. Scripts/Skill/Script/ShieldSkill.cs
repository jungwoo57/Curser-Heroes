using UnityEngine;

public class RotatingShieldSkill : MonoBehaviour
{
    [SerializeField] private GameObject shieldPrefab;

    private Transform player;
    private float rotateSpeed = 100f;
    private float radius = 1.5f;

    public void Init(SkillManager.SkillInstance skillInstance, Transform playerTransform)
    {
        var levelData = skillInstance.skill.levelDataList[skillInstance.level - 1];
        player = playerTransform;

        int shieldCount = (skillInstance.level >= skillInstance.skill.maxLevel) ? 2 : 1;
        float angleStep = 360f / shieldCount;
        float scaleMultiplier = Mathf.Pow(1.1f, skillInstance.level - 1);

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
    }

    void Update()
    {
        if (player != null)
        {
            transform.position = player.position;
            transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
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