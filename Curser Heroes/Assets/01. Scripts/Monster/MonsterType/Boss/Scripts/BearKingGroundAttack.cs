using System;
using System.Collections;
using UnityEngine;

public class BearKingGroundAttack : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;
    [SerializeField] private float deActiveTime;
    private Vector3 dir;


    private void OnEnable()
    {
        StartCoroutine(SelfDeactive());
        animator.SetTrigger("Idle");
    }

    private void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
    }

    public void Init(Vector3 dir)
    {
        this.dir = dir;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            // 무기 내구도 감소
            if (WeaponManager.Instance != null && !WeaponManager.Instance.isInvincible)
            {
                WeaponManager.Instance.TakeWeaponLifeDamage();
                gameObject.SetActive(false);
            }
        }
    }

    IEnumerator SelfDeactive()
    {
        yield return new WaitForSeconds(deActiveTime);
        gameObject.SetActive(false);
    }
}
