using UnityEngine;
using System.Collections;

public class BearKingBoss : BossBaseMonster
{
    [Header("Pattern1 - Linear Smash")]
    public GameObject linearWavePrefab;
    public Transform smashOrigin;

    [Header("Pattern2 - Jump Smash")]
    public GameObject jumpIndicatorPrefab;
    public GameObject jumpImpactPrefab;

    [Header("Pattern3 - Melee Swipe")]
    public float meleeRange = 1.5f;
    public LayerMask targetLayer;

    private static readonly int HashPattern1 = Animator.StringToHash("Pattern1");
    private static readonly int HashPattern2 = Animator.StringToHash("Pattern2");
    private static readonly int HashPattern3 = Animator.StringToHash("Pattern3");

    protected override IEnumerator Pattern1()
    {
        animator?.SetBool(HashPattern1, true);

        yield return new WaitForSeconds(0.8f);

        if (linearWavePrefab != null && smashOrigin != null)
        {
            Instantiate(linearWavePrefab, smashOrigin.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(1.2f);
        animator?.SetBool(HashPattern1, false);
    }

    protected override IEnumerator Pattern2()
    {
        animator?.SetBool(HashPattern2, true);

        yield return new WaitForSeconds(2f);

        Vector3 targetPos = transform.position;
        if (GameObject.FindGameObjectWithTag("Weapon") != null)
        {
            targetPos = GameObject.FindGameObjectWithTag("Weapon").transform.position;
        }

        if (jumpIndicatorPrefab != null)
        {
            GameObject indicator = Instantiate(jumpIndicatorPrefab, targetPos, Quaternion.identity);
            Destroy(indicator, 1f);
        }

        yield return new WaitForSeconds(1f);

        transform.position = targetPos;

        if (jumpImpactPrefab != null)
        {
            Instantiate(jumpImpactPrefab, targetPos, Quaternion.identity);
        }

        animator?.SetBool(HashPattern2, false);
    }

    protected override IEnumerator Pattern3()
    {
        animator?.SetBool(HashPattern3, true);

        yield return new WaitForSeconds(1f);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, meleeRange, targetLayer);
        foreach (var hit in hits)
        {
            Debug.Log("Hit: " + hit.name);
        }

        yield return new WaitForSeconds(1f);

        animator?.SetBool(HashPattern3, false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}