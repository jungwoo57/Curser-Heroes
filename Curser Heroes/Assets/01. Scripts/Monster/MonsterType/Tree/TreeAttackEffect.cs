using System.Collections;
using UnityEngine;

public class TreeAttackEffect : MonoBehaviour
{
    public float DestroyDelay = 1;            // 전체 지속 시간
    public float stepDelay = 0.1f;          // 자식 하나씩 켜는 간격

    private void Start()
    {
        StartCoroutine(ActivateChildrenStepByStep());
        Destroy(gameObject, DestroyDelay);
    }

    private IEnumerator ActivateChildrenStepByStep()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            yield return new WaitForSeconds(stepDelay);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            if (WeaponManager.Instance != null && !WeaponManager.Instance.isInvincible)
            {
                WeaponManager.Instance.TakeWeaponLifeDamage();
                Debug.Log("뿌리 충돌: 무기 내구도 감소!");
                Destroy(gameObject);
            }
        }
    }
}
