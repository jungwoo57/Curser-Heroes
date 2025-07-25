using UnityEngine;
using System.Collections;

public class MirrorCursorSkill : MonoBehaviour
{
    [SerializeField] private GameObject mirrorPrefab;
    [SerializeField] private float xOffset = 1.5f;

    private float lastSpawnTime = -50f;
    private SkillManager.SkillInstance skillInstance;
    private Transform cursorTransform;

    private float spawnInterval => skillInstance?.GetCurrentLevelData().cooldown ?? 30f;
    private float mirrorDuration => skillInstance?.GetCurrentLevelData().duration ?? 5f;

    public void Init(SkillManager.SkillInstance instance, Transform cursor)
    {
        skillInstance = instance;
        cursorTransform = cursor;
        lastSpawnTime = -spawnInterval; // 웨이브 시작 즉시 발동 가능
    }

    private void Update()
    {
        if (Time.time - lastSpawnTime >= spawnInterval)
        {
            SpawnMirrors();
            lastSpawnTime = Time.time;
        }
    }

    private void SpawnMirrors()
    {
        CursorWeapon cursorWeapon = FindObjectOfType<CursorWeapon>();
        if (cursorWeapon == null)
        {
            Debug.LogWarning("CursorWeapon을 찾을 수 없습니다.");
            return;
        }

        int baseDamage = cursorWeapon.GetCurrentDamage();
        int damageInt = Mathf.Clamp(skillInstance.GetCurrentLevelData().damage, 0, 10);
        float damageCoefficient = damageInt * 0.1f;
        float finalDamage = baseDamage * damageCoefficient;

        Debug.Log($"[MirrorCursorSkill] baseDamage = {baseDamage}, damageInt = {damageInt}, finalDamage = {finalDamage}");

        Vector3 leftOffset = Vector3.left * xOffset;
        Vector3 rightOffset = Vector3.right * xOffset;

        CreateMirror(leftOffset, finalDamage);
        CreateMirror(rightOffset, finalDamage);
    }

    private void CreateMirror(Vector3 offset, float damage)
    {
        Vector3 spawnPos = cursorTransform.position + offset;
        GameObject mirror = Instantiate(mirrorPrefab, spawnPos, Quaternion.identity);
        mirror.GetComponent<MirrorCursor>().Init(cursorTransform, damage, mirrorDuration, offset);
    }
}