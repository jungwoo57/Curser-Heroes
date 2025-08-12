using UnityEngine;

public class WarningIndicator : MonoBehaviour
{
    public Transform target;
    public float duration = 0.5f;
    public Color targetColor = new Color(1f, 0f, 0f, 0.8f);

    private SpriteRenderer sr;
    private float timer;

    private readonly Color startColor = new Color(1f, 0f, 0f, 0f); // 투명 빨강

    private void OnEnable()
    {
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();

        timer = 0f;
        sr.color = startColor;
    }

    private void Update()
    {
        if (target == null) return;

        transform.position = target.position; // enable로 옮겨보자

        timer += Time.deltaTime;

        float t = Mathf.Clamp01(timer / duration);
        sr.color = Color.Lerp(startColor, targetColor, t); 

        if (timer >= duration)
            gameObject.SetActive(false);
    }
}
