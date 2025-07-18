using UnityEngine;
using System.Collections;

public class Messengerbird : BasePartner
{
    public GameObject bombPrefab;
    public float activeDuration = 7f;
    public float explosionDelay = 1f;
    public float explosionRadius = 2f;
    public float damageMultiplier = 2.2f;

    private bool isActive = false;

    public BirdChirpEffect birdChirpEffect; // 새 효과 참조

    protected override void ActivateSkill()
    {
        StartCoroutine(SkillRoutine());
        currentGauge = 0f;
        ui.UpdateGauge(0f);

        if (birdChirpEffect != null)
            birdChirpEffect.PlayChirp(); // 스킬 시작과 함께 새 연출 실행
    }

    private IEnumerator SkillRoutine()
    {
        isActive = true;

        yield return new WaitForSeconds(activeDuration);
        isActive = false;
    }

    public void HandleMonsterDeath(GameObject monster, int damage)
    {
        if (!isActive || bombPrefab == null) return;

        GameObject bomb = Instantiate(bombPrefab, monster.transform.position, Quaternion.identity);
        DelayedBomb bombScript = bomb.GetComponent<DelayedBomb>();
        bombScript.Initialize(damage * damageMultiplier, explosionRadius);
    }
}