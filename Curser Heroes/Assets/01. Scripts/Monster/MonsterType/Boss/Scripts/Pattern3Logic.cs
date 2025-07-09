using System.Collections;
using UnityEngine;

public class Pattern3Logic : PatternLogicBase
{
    public Transform targetObject1;
    public float redFadeDuration = 1.8f;

    public override IEnumerator Execute(BossPatternController controller)
    {
        if (targetObject1 == null)
        {
            Debug.LogWarning("Pattern3Logic: targetObject가 할당되지 않았습니다.");
            yield break;
        }

        SpriteRenderer sr = targetObject1.GetComponentInChildren<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogWarning("자식 오브젝트에 SpriteRenderer가 없습니다.");
            yield break;
        }

        Color originalColor = sr.color;

        // 빨갛게 서서히
        yield return StartCoroutine(ChangeColorGradually(sr, originalColor, Color.red, redFadeDuration));

      
        sr.color = originalColor;
    }

    private IEnumerator ChangeColorGradually(SpriteRenderer sr, Color fromColor, Color toColor, float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            sr.color = Color.Lerp(fromColor, toColor, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        sr.color = toColor;
    }
}
