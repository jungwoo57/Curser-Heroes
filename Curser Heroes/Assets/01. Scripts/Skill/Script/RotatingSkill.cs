using UnityEngine;

public class RotatingSkill : MonoBehaviour
{
    public GameObject rotatingObjectPrefab; // 새 필드 추가

    private Transform player;
    private float rotateSpeed = 90f;
    private float radius = 1f;

    public void Init(SkillManager.SkillInstance skillInstance, Transform playerTransform)
    {
        var levelData = skillInstance.skill.levelDataList[skillInstance.level - 1];
        player = playerTransform;

        int count = levelData.count;
        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

            Quaternion rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
            GameObject obj = Instantiate(rotatingObjectPrefab, transform.position + offset, rotation, transform);
            obj.transform.localScale *= levelData.sizeMultiplier;

            if (obj.TryGetComponent(out SkillProjectile proj))
                proj.Init(levelData.damage, Vector2.zero, 0);
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

    public void UpdateSwords(SkillManager.SkillInstance skillInstance)
    {
        // 기존 블레이드 제거
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Init 호출로 새롭게 블레이드 재배치
        Init(skillInstance, player);
    }
}