using UnityEngine;
using System.Collections;

public class BirdChirpEffect : MonoBehaviour
{
    public GameObject birdImage;     // 새 이미지 게임오브젝트 (초기엔 SetActive(false))
    public Transform startPos;       // 화면 오른쪽 밖 위치
    public Transform visiblePos;     // 새가 보일 위치
    public float moveDuration = 1f;
    public float visibleDuration = 6f;
    public AudioSource chirpAudio;

    private void Awake()
    {
        if (birdImage != null)
            birdImage.SetActive(false);
        gameObject.SetActive(false); // 기본 꺼짐
    }

    public void PlayChirp()
    {
        Debug.Log("PlayChirp 시작");
        gameObject.SetActive(true);
        if (birdImage != null)
        {
            birdImage.SetActive(true);
            birdImage.transform.position = startPos.position;
            StartCoroutine(MoveBirdRoutine());
        }
        if (chirpAudio != null)
            chirpAudio.Play();
        StartCoroutine(HideAfterDelay());
    }

    private IEnumerator MoveBirdRoutine()
    {
        float timer = 0f;
        while (timer < moveDuration)
        {
            birdImage.transform.position = Vector3.Lerp(startPos.position, visiblePos.position, timer / moveDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        birdImage.transform.position = visiblePos.position;

        yield return new WaitForSeconds(visibleDuration);

        timer = 0f;
        while (timer < moveDuration)
        {
            birdImage.transform.position = Vector3.Lerp(visiblePos.position, startPos.position, timer / moveDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        birdImage.transform.position = startPos.position;
        birdImage.SetActive(false);
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(chirpAudio.clip.length);
        Debug.Log("PlayChirp 끝");
        gameObject.SetActive(false);
    }
}