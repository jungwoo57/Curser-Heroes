using System.Collections;
using UnityEngine;

public class BurnEffect : MonoBehaviour, ISubWeaponEffect
{
    public float burnDamage = 0.5f;
    public float duration = 3f;
    public float interval = 0.5f;

    public void ApplyEffect(BaseMonster target, float damage)
    {
        target.TakeDamage(Mathf.RoundToInt(damage));
        target.StartCoroutine(ApplyBurn(target));         //데미지를 먼저 주고 상태이상(화상)부여
    }

    private IEnumerator ApplyBurn(BaseMonster target)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (target == null) yield break;
            target.TakeDamage(Mathf.RoundToInt(burnDamage));
            elapsed += interval;
            yield return new WaitForSeconds(interval);
        }
    }
}
