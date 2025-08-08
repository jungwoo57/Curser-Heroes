using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarUI : MonoBehaviour
{
    
   [SerializeField] private TutorialImageUI tutorialImageUI;
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
