using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarUI : MonoBehaviour
{
    [SerializeField] private TutorialUI tutorialUI;
    [SerializeField] private GameObject tutorialPanel;
    private void OnEnable()
    {
        tutorialUI.gameObject.SetActive(false);
        tutorialPanel.SetActive(false);
        if (!GameManager.Instance.useBar)
        {
            tutorialPanel.SetActive(false);
            tutorialUI.gameObject.SetActive(true);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Exit();
        }
    }
    
    
    public void Exit()
    {
        gameObject.SetActive(false);
    }
}
