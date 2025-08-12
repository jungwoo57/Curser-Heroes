using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octopus_Ink : MonoBehaviour
{
    public float speed;
    private Vector3 direction;
    private int damage;
    [SerializeField] private GameObject darkImage;
    [SerializeField] private float blackTime;
    // 투사체 초기화 (방향, 데미지)
    public void Initialize(Vector3 dir, int dmg)
    {
        direction = dir.normalized;
        damage = dmg;
        darkImage.SetActive(false);
        Destroy(gameObject, 1f); // 3초 후 자동 파괴
    }

    void Update()
    {
        // 매 프레임 방향으로 이동
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 'Weapon' 레이어와 충돌 시
        if (collision.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            // 무기 내구도 감소
            if (WeaponManager.Instance != null && !WeaponManager.Instance.isInvincible)
            {
                StartCoroutine(ActiveBlack());
                WeaponManager.Instance.TakeWeaponLifeDamage();
                Debug.Log("투사체 충돌: 무기 내구도 감소!");
                Destroy(gameObject); // 즉시 파괴, 충돌했을 때만
            }

            //Destroy(gameObject); // 즉시 파괴
        }
    }

    IEnumerator ActiveBlack()
    {
        darkImage.transform.position = new Vector3(0, 0, 0);
        darkImage.SetActive(true);
        yield return new WaitForSeconds(blackTime);
        darkImage.SetActive(false);
    }
}
