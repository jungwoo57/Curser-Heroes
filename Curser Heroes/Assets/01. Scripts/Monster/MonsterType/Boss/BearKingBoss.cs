// BearKingBoss.cs (unchanged except 패턴 내부 null 체크 유지)
using UnityEngine;
using System.Collections;

public class BearKingBoss : BossBaseMonster
{
    private static readonly int HashP1 = Animator.StringToHash("Pattern1");
    private static readonly int HashP2 = Animator.StringToHash("Pattern2");
    private static readonly int HashP3 = Animator.StringToHash("Pattern3");

    [Header("Pattern1 - Linear Smash")]
    public GameObject linearWavePrefab;
    public Transform smashOrigin;

    [Header("Pattern2 - Jump Smash")]
    public GameObject jumpIndicatorPrefab;
    public GameObject jumpImpactPrefab;

    [Header("Pattern3 - Melee Swipe")]
    public float meleeRange = 1.5f;

    protected override IEnumerator Pattern1()
    {
        animator?.SetBool(HashP1, true);
        if (smashOrigin == null || linearWavePrefab == null)
            Debug.LogWarning("Pattern1: smashOrigin 또는 linearWavePrefab 미설정");

        yield return new WaitForSeconds(1f);

        if (smashOrigin != null && linearWavePrefab != null)
            Instantiate(linearWavePrefab, smashOrigin.position, Quaternion.identity);

        if (smashOrigin != null)
        {
            var hits = Physics2D.OverlapCircleAll(smashOrigin.position, 2f, LayerMask.GetMask("Weapon"));
            foreach (var c in hits)
                WeaponManager.Instance?.TakeWeaponLifeDamage();
        }

        yield return new WaitForSeconds(1f);
        animator?.SetBool(HashP1, false);
    }

    protected override IEnumerator Pattern2()
    {
        animator?.SetBool(HashP2, true);
        yield return new WaitForSeconds(1.5f);

        Vector3 targetPos = transform.position;
        var weaps = Physics2D.OverlapCircleAll(transform.position, 100f, LayerMask.GetMask("Weapon"));
        float md = Mathf.Infinity;
        foreach (var w in weaps)
        {
            if (w == null) continue;
            float d = (w.transform.position - transform.position).sqrMagnitude;
            if (d < md)
            {
                md = d;
                targetPos = w.transform.position;
            }
        }

        if (jumpIndicatorPrefab != null)
        {
            var ind = Instantiate(jumpIndicatorPrefab, targetPos, Quaternion.identity);
            Destroy(ind, 1f);
        }

        yield return new WaitForSeconds(0.5f);

        Vector3 start = transform.position;
        float t = 0f;
        while (t < 1f)
        {
            transform.position = Vector3.Lerp(start, targetPos, t);
            t += Time.deltaTime * 2f;
            yield return null;
        }
        transform.position = targetPos;

        if (jumpImpactPrefab != null)
            Instantiate(jumpImpactPrefab, targetPos, Quaternion.identity);

        var impactHits = Physics2D.OverlapCircleAll(targetPos, 2f, LayerMask.GetMask("Weapon"));
        foreach (var c in impactHits)
            WeaponManager.Instance?.TakeWeaponLifeDamage();

        yield return new WaitForSeconds(0.5f);
        animator?.SetBool(HashP2, false);
    }

    protected override IEnumerator Pattern3()
    {
        animator?.SetBool(HashP3, true);
        yield return new WaitForSeconds(0.5f);

        if (meleeRange <= 0f)
            Debug.LogWarning("Pattern3: meleeRange가 0 이하");

        var hits = Physics2D.OverlapCircleAll(transform.position, meleeRange, LayerMask.GetMask("Weapon"));
        foreach (var c in hits)
            WeaponManager.Instance?.TakeWeaponLifeDamage();

        yield return new WaitForSeconds(0.5f);
        animator?.SetBool(HashP3, false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (smashOrigin != null)
            Gizmos.DrawWireSphere(smashOrigin.position, 2f);
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}
