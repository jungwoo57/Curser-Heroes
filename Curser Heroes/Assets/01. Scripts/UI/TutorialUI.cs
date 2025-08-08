using System;
using UnityEngine.Video;
using UnityEngine;


public class TutorialUI : MonoBehaviour
{  
   private enum tutorialType{stage, bar, forge, lab}
   [SerializeField] VideoPlayer videoPlayer;
   [SerializeField] GameObject videoPanel;
   [SerializeField] private tutorialType type;

   public Action tutorialEnd;
   private void Awake()
   {
      videoPlayer.loopPointReached += VideoEnd;
   }
   
   private void OnEnable()
   {
      videoPanel.SetActive(true);
      videoPlayer.Play();
      
   }
   
   private void VideoEnd(VideoPlayer vp)
   {
      switch (type)
      {
         case tutorialType.stage:
            GameManager.Instance.useStage = true;
            break;
         case tutorialType.bar: 
            GameManager.Instance.useBar = true;
            break;
         case tutorialType.forge:
            GameManager.Instance.useForge = true;
            break;
         case tutorialType.lab: 
            GameManager.Instance.useLab = true;
            break;
         default: break;   
      }
      tutorialEnd?.Invoke();
      videoPanel.SetActive(false);
   }
}
