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
   [SerializeField] private TextMeshProUGUI countText;

   private Vector2 startPos = new Vector2(1280, 0);        //스테이지 텍스트 시작 위치
   private Vector2 endPos = new Vector2(-1280, 0);         //스테이지 텍스트 끝나는 위치

   [Header("연출 시간")]
   [SerializeField] private float moveDurationTime;   //중앙 까지 움직이는 시간
   [SerializeField] private float slowDurationTime;   // 중앙에서 느려지는 시간

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
      startText.gameObject.SetActive(false);
   }

   [ContextMenu("애니메이션 시작")]
   public void StartAnimation()
   {
      StartCoroutine(StageStartAnimation());
   }
   
   IEnumerator StageStartAnimation()
   {
      float time = 0;
      Debug.Log(startPos.x - slowStartPos.x);
      while (time < moveDurationTime)
      {
         time += Time.deltaTime;
         float lerp = time / moveDurationTime;
         //Debug.Log(stageText.rectTransform.anchoredPosition.x);
         stageText.rectTransform.anchoredPosition = Vector2.MoveTowards(startPos, slowStartPos, lerp);
         yield return null;
      }

      /*time = 0;
      while (time < slowDurationTime)
      {
         time += Time.deltaTime;
         float lerp = time / slowDurationTime;
         stageText.rectTransform.anchoredPosition = Vector2.Lerp(slowStartPos,slowEndPos, lerp);
         yield return null;
      }*/

      time = 0;
      Debug.Log(slowEndPos.x - endPos.x);
      while (time < moveDurationTime)
      {
         time += Time.deltaTime;
         float lerp = time / moveDurationTime;
         stageText.rectTransform.anchoredPosition = Vector2.Lerp(slowEndPos,endPos, lerp);
         yield return null;
      }

      //Init();
   }
   
}
