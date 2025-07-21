using UnityEngine;

public class SporeExplosionSkill : MonoBehaviour
{
    private SkillManager.SkillInstance skillInstance;
    public GameObject projectilePrefab;

    private float procChance = 0.25f;
    private float projectileSpeed = 2f;

    public void Init(SkillManager.SkillInstance instance)
    {
        skillInstance = instance;
        var levelData = skillInstance.GetCurrentLevelData();
    }

    public void TryTrigger(Vector3 position)
    {
        if (skillInstance == null)
        {
            return;
        }

        float roll = Random.value;
        Debug.Log($"[SporeExplosionSkill] 프로크 확률: {procChance}, 난수: {roll}");

        if (roll > procChance)
        {
            Debug.Log("[SporeExplosionSkill] 스킬 발동 실패 (확률 실패)");
            return;
        }

        int damage = skillInstance.GetCurrentLevelData().damage;
        Debug.Log($"[SporeExplosionSkill] 스킬 발동! 데미지: {damage}");

        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        foreach (Vector2 dir in directions)
        {
            Quaternion rot = Quaternion.identity;

            if (dir == Vector2.up) rot = Quaternion.Euler(0f, 0f, 90f);
            else if (dir == Vector2.down) rot = Quaternion.Euler(0f, 0f, -90f);
            else if (dir == Vector2.left) rot = Quaternion.Euler(0f, 0f, 180f);
            else if (dir == Vector2.right) rot = Quaternion.Euler(0f, 0f, 0f);

            GameObject proj = Instantiate(projectilePrefab, position, rot);
            Debug.Log($"[SporeExplosionSkill] 포자 투사체 생성: 방향={dir.normalized}, 회전={rot.eulerAngles}");

            SporeProjectile sp = proj.GetComponent<SporeProjectile>();
            sp.Init(damage, dir.normalized, projectileSpeed);
        }

    }
}