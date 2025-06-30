using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class StageStartUI : MonoBehaviour
{
   [Header("텍스트")]
   [SerializeField] private TextMeshProUGUI startText;
   [SerializeField] private TextMeshProUGUI stageText;

   [SerializeField]private Vector2 startPos = new Vector2(1280, 0);        //스테이지 텍스트 시작 위치
   [SerializeField]private Vector2 endPos = new Vector2(-1280, 0);         //스테이지 텍스트 끝나는 위치

   [Header("연출 시간")]
   [SerializeField] private float moveDurationTime;   //중앙 까지 움직이는 시간
   [SerializeField] private float slowDurationTime;   // 중앙에서 느려지는 시간
   [SerializeField] private float startTextScaleUpTime;
   [Header("슬로우 영역")] 
   [SerializeField] private Vector2 slowStartPos = new Vector2(150, 0);
   [SerializeField] private Vector2 slowEndPos = new Vector2(-150, 0);
   
   private void Awake()
   {
      Init();
   }


   public void Init()
   {
      stageText.rectTransform.anchoredPosition =startPos;
   }

   [ContextMenu("애니메이션 시작")]
   public void StartAnimation()
   {
      gameObject.SetActive(true);
      StartCoroutine(AnimationSequence());
   }

   IEnumerator AnimationSequence()
   {
      yield return StartCoroutine(StageStartAnimation("STAGE 1")); // 해당 부분 현재 스테이지 이름이로 추후 이름변경
      
      yield return StartCoroutine(StageStartAnimation("1"));
      
      yield return StartCoroutine(StageStartAnimation("2"));
      
      yield return StartCoroutine(StageStartAnimation("3"));

      yield return StartCoroutine(StartTextAnimation());
      
   }
   
   IEnumerator StageStartAnimation(string letter)
   {
      stageText.rectTransform.anchoredPosition = startPos;
      stageText.text = letter;
      yield return null;
      float time = 0;
      while (time < moveDurationTime)
      {
         float lerp = time / moveDurationTime;
         stageText.rectTransform.anchoredPosition = Vector2.Lerp(startPos, slowStartPos, lerp);
         yield return null;
         time += Time.deltaTime;
      }

      time = 0;
      while (time < slowDurationTime)
      {
         time += Time.deltaTime;
         float lerp = time / slowDurationTime;
         stageText.rectTransform.anchoredPosition = Vector2.Lerp(slowStartPos,slowEndPos, lerp);
         yield return null;
      }

      time = 0;
      while (time < moveDurationTime)
      {
         time += Time.deltaTime;
         float lerp = time / moveDurationTime;
         stageText.rectTransform.anchoredPosition = Vector2.Lerp(slowEndPos,endPos, lerp);
         yield return null;
      }
   }

   IEnumerator StartTextAnimation()
   {
      float time = 0;
      while (time < startTextScaleUpTime)
      {
         float lerp = time/startTextScaleUpTime;
         startText.rectTransform.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, lerp);
         time += Time.deltaTime;
         yield return null;
      }
      startText.rectTransform.localScale = Vector2.zero;
      WaveManager.Instance.StartWave();
      yield return null;
      gameObject.SetActive(false);
   }
}
