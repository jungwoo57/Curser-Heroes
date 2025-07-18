using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmyEffect : MonoBehaviour
{
    [Header("이펙트 세팅")]
    public GameObject bulletPrefab;         // 총알 프리팹
    public GameObject shooterPrefab;        // 총 이미지(총알 발사체)
    public GameObject backgroundDarkener;   // 어두워지는 배경 (UI Panel)

    [Header("발사 설정")]
    public int bulletsPerShoot = 8;         // 한 번에 쏘는 총알 개수
    public int shootTimes = 4;               // 총 몇 번 쏠지
    public float shootInterval = 0.5f;      // 발사 간격(초)
    public float radius = 5f;                // (0,0) 기준 총알/발사체 위치 반지름

    [Header("페이드 설정")]
    public float fadeDuration = 0.5f;          // 페이드 인/아웃 시간
    public float delayBeforeFire = 0.5f;     // 총알 발사 전 대기시간
    public float durationAfterFire = 0.5f;     // 발사 후 연출 유지 시간

    public void PlayEffect(Vector3 center)
    {
        StartCoroutine(EffectRoutine(center));
    }

    private IEnumerator EffectRoutine(Vector3 center)
    {
        GameObject bg = null;
        Image bgImage = null;

        // 1. UI Canvas 찾기
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("씬에 Canvas가 없습니다. backgroundDarkener가 보이지 않을 수 있습니다.");
        }

        // 2. 어두워짐 효과용 배경 생성 (Canvas 자식으로)
        if (backgroundDarkener != null && canvas != null)
        {
            bg = Instantiate(backgroundDarkener, Vector3.zero, Quaternion.identity);
            bg.transform.SetParent(canvas.transform, false);  // Canvas 하위에 붙임
            bg.transform.localPosition = Vector3.zero;
            bg.transform.localScale = Vector3.one;

            bgImage = bg.GetComponent<Image>();
            if (bgImage != null)
            {
                bgImage.color = new Color(0, 0, 0, 0f);
                Debug.Log("[ArmyEffect] 페이드 인 시작");
            }
            else
            {
                Debug.LogWarning("backgroundDarkener 프리팹에 Image 컴포넌트가 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("[ArmyEffect] backgroundDarkener 또는 Canvas가 할당/발견되지 않았습니다.");
        }

        // 3. 페이드 인 (어두워지기)
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            if (bgImage != null)
                bgImage.color = new Color(0, 0, 0, Mathf.Lerp(0f, 0.8f, t / fadeDuration));
            yield return null;
        }

        // 4. 딜레이 (발사 전)
        yield return new WaitForSeconds(delayBeforeFire);

        // 5. 총알 쏘는 위치에 shooter(총 이미지) 생성
        List<Transform> shooters = new List<Transform>();
        for (int i = 0; i < bulletsPerShoot; i++)
        {
            float angle = 360f / bulletsPerShoot * i;
            Vector3 pos = new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad),
                0f) * radius;

            GameObject shooterObj = Instantiate(shooterPrefab, pos, Quaternion.identity, transform);
            Vector3 dirToCenter = -pos.normalized;
            float angleZ = Mathf.Atan2(dirToCenter.y, dirToCenter.x) * Mathf.Rad2Deg;
            shooterObj.transform.rotation = Quaternion.Euler(0, 0, angleZ);
            shooters.Add(shooterObj.transform);
        }

        // 6. 0.3초 간격으로 총알 여러번 발사
        for (int shootRound = 0; shootRound < shootTimes; shootRound++)
        {
            foreach (Transform shooter in shooters)
            {
                GameObject bullet = Instantiate(bulletPrefab, shooter.position, Quaternion.identity);
                Vector2 dir = ((Vector2)Vector3.zero - (Vector2)shooter.position).normalized;

                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                    rb.velocity = dir * 10f;

                Destroy(bullet, 0.5f);
            }

            yield return new WaitForSeconds(shootInterval);
        }

        // 7. shooter 오브젝트 제거
        foreach (Transform shooter in shooters)
        {
            Destroy(shooter.gameObject);
        }

        // 8. 총알 발사 후 지정된 시간만큼 대기
        yield return new WaitForSeconds(durationAfterFire);

        // 9. 페이드 아웃 (밝아지기)
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            if (bgImage != null)
                bgImage.color = new Color(0, 0, 0, Mathf.Lerp(0.8f, 0f, t / fadeDuration));
            yield return null;
        }
        Debug.Log("[ArmyEffect] 페이드 아웃 완료");

        // 10. 배경과 자기 자신 정리
        if (bg != null) Destroy(bg);
        Destroy(gameObject);
    }
}