using UnityEngine;

public class PredatorSkill : MonoBehaviour
{
    [SerializeField] private GameObject effectPrefab;

    private SkillManager.SkillInstance skillInstance;
    private Transform cursorTransform;

    public void Init(SkillManager.SkillInstance instance, Transform cursorTransform)
    {
        skillInstance = instance;
        this.cursorTransform = cursorTransform;
    }

    public void OnMonsterKilled(Vector3 spawnPosition)
    {
        if (skillInstance == null) return;

        float chance = skillInstance.GetCurrentLevelData().count*0.01f;
        if (Random.value < chance && WeaponManager.Instance.weaponLife)
        {
            WeaponManager.Instance.weaponLife.RecoverLife();
            PlayEffect();
        }
    }

    private void PlayEffect()
    {
        if (effectPrefab == null || cursorTransform == null) return;

        Vector3 pos = cursorTransform.position;
        GameObject effect = Instantiate(effectPrefab, pos, Quaternion.identity);
        effect.transform.SetParent(cursorTransform);
        effect.transform.localPosition = new Vector3(0, 0, -1); // 앞쪽에 위치
    }
}