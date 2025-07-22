using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarUI : MonoBehaviour
{
    [SerializeField]TutorialUI tutorialUI;

    private void OnEnable()
    {
        tutorialUI.gameObject.SetActive(false);
        if (!GameManager.Instance.useBar)
        {
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
