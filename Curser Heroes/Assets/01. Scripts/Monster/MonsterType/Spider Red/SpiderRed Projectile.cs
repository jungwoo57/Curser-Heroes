using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderRedProjectile : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 direction;
    private int damage;
    private bool canSplit = true;

    [Header("Split Settings")]
    public GameObject splitProjectilePrefab;
    public float splitDelay = 0.15f;              // 여기서 분열 타이밍 조절 (0.1~0.2f 추천)
    public float splitChildLifeTime = 2f;

    private float splitTimer;

    public void Initialize(Vector2 dir, int dmg, bool allowSplit = true)
    {
        direction = dir.normalized;
        damage = dmg;
        canSplit = allowSplit;

        if (canSplit)
        {
            splitTimer = splitDelay; //  약간 기다렸다 분열
        }
        else
        {
            Destroy(gameObject, splitChildLifeTime); // 자식은 2초 후 제거
        }
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        if (canSplit)
        {
            splitTimer -= Time.deltaTime;
            if (splitTimer <= 0f)
            {
                Split();
                canSplit = false;
            }
        }
    }

    private void Split()
    {
        if (splitProjectilePrefab == null) return;

        Vector2[] splitDirs = new Vector2[]
        {
            Rotate2D(direction, -20f),
            direction,
            Rotate2D(direction, 20f)
        };

        foreach (Vector2 dir in splitDirs)
        {
            GameObject newProj = Instantiate(splitProjectilePrefab, transform.position, Quaternion.identity);
            DirtProjectile projScript = newProj.GetComponent<DirtProjectile>();
            if (projScript != null)
            {
                projScript.Initialize(dir, damage, false); // 자식은 분열 X
            }
        }

        Destroy(gameObject); // 원본 제거
    }

    private Vector2 Rotate2D(Vector2 dir, float angleDeg)
    {
        float rad = angleDeg * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(
            dir.x * cos - dir.y * sin,
            dir.x * sin + dir.y * cos
        ).normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            if (WeaponManager.Instance != null && !WeaponManager.Instance.isInvincible)
            {
                WeaponManager.Instance.TakeWeaponLifeDamage();
                Debug.Log("투사체 충돌: 무기 내구도 감소!");
            }

            Destroy(gameObject);
        }
    }
}
