using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMainLoadManager : MonoBehaviour
{
    public static TitleMainLoadManager Instance;
    public CanvasGroup fadeImage;
    
    [SerializeField]private float duration; // 페이드아웃 시간
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadMain()
    {
        StartCoroutine(LoadScene());
    }
    
    private IEnumerator LoadScene()
    {
        yield return StartCoroutine(FadeOut());

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("JW_StageSelectUI");
        while(!asyncLoad.isDone)
        {
            yield return null;
        }

        yield return StartCoroutine(FadeIn());
    }

    private IEnumerator FadeOut()
    {
        fadeImage.blocksRaycasts = true;
        float t = 0;
        while (t < duration)
        {
            fadeImage.alpha = Mathf.Lerp(0, 1.0f, t / duration);
            yield return null;
            t += Time.deltaTime;
        }

        fadeImage.alpha = 1.0f;
    }

    private IEnumerator FadeIn()
    {
        float t = 0;
        while (t < duration)
        {
            fadeImage.alpha = Mathf.Lerp(1.0f, 0, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        fadeImage.blocksRaycasts = false;
        fadeImage.alpha = 0;
    }
    
}
