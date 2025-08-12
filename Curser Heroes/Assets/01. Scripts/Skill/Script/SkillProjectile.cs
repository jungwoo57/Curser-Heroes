using UnityEngine;

public class SkillProjectile : MonoBehaviour
{
    private RotatingSkill parentSkill;
    private int damage;

    public void Init(RotatingSkill parent, int dmg)
    {
        parentSkill = parent;
        damage = dmg;
    }

    // 충돌이 시작되는 순간에만 호출됩니다.
    private void OnTriggerEnter2D(Collider2D other)
    {
        BaseMonster monster = other.GetComponent<BaseMonster>();
        if (monster != null)
        {
            parentSkill.ApplyDamageAndDisableCollision(other);
        }
        else
        {
            BossStats boss = other.GetComponent<BossStats>();
            if (boss != null)
            {
                parentSkill.ApplyDamageAndDisableCollision(other);
            }
        }
    }
}