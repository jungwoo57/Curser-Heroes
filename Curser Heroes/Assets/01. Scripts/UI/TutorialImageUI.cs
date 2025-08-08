using System;
using UnityEngine;
using UnityEngine.UI;
public class TutorialImageUI : MonoBehaviour
{
    [SerializeField] private Image baseImage;
    [SerializeField] private Sprite[] tutorialImages;
    [SerializeField] private Button tutorialButtonRight;
    [SerializeField] private Button tutorialButtonLeft;
    [SerializeField] private int curIndex = 0;


    private void OnEnable()
    {
        curIndex = 0;
        UpdateUI();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ExitTutorialPanel();
        }
    }

    private void UpdateUI()
    {
        if (curIndex == 0)
        {
            tutorialButtonLeft.gameObject.SetActive(false);
        }
        else
        {
            tutorialButtonLeft.gameObject.SetActive(true);
        }
        if (curIndex >= tutorialImages.Length-1)
        {
            tutorialButtonRight.gameObject.SetActive(false);
        }
        else
        {
            tutorialButtonRight.gameObject.SetActive(true);
        }
        baseImage.sprite = tutorialImages[curIndex];
    }

    public void ClickNextButton(int i)
    {
        curIndex += i;
        if (curIndex <= 0)
        {
            curIndex = 0;
        }

        if (curIndex >= tutorialImages.Length)
        {
            curIndex = tutorialImages.Length-1 ;
        }
        
        UpdateUI();
    }
    
    public void ExitTutorialPanel()
    {
        gameObject.SetActive(false);
    }
    
    
}
