using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarUI : MonoBehaviour
{
    
   [SerializeField] private TutorialImageUI tutorialImageUI;

   private void OnEnable()
   {
       if (!GameManager.Instance.useBar)
       {
           ClickHintButton();
           GameManager.Instance.useBar = true;
           GameManager.Instance.Save();
       }
   }

   private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !tutorialImageUI.gameObject.activeInHierarchy)
        {
            Exit();
        }
    }
    
    public void ClickHintButton()
    {
        tutorialImageUI.gameObject.SetActive(true);
    }
    
    public void Exit()
    {
        gameObject.SetActive(false);
    }
}
