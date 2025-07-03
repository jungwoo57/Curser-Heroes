using System;
using UnityEngine;
using UnityEngine.UI;
public class VillageUI : MonoBehaviour
{
    [Header("메인 UI")]
    [SerializeField] private Button stageEntryButton;
    [SerializeField] private Button laboratoryEntryButton;
    [SerializeField] private Button barEntryButton;
    [SerializeField] private Button forgeEntryButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Image menuImage;

    [Header("실행창 모음")] 
    [SerializeField] private GameObject stageEntryPanel;
    [SerializeField] private GameObject laboratoryEntryPanel;
    [SerializeField] private GameObject barEntryPanel;
    [SerializeField] private GameObject forgeEntryPanel;
    [SerializeField] private GameObject menuPanel;

    private void Start()
    {
        Init();
    }
    
    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        stageEntryPanel.SetActive(false);
        //laboratoryEntryPanel.SetActive(false);
        //barEntryPanel.SetActive(false);
        //forgeEntryPanel.SetActive(false);
        menuPanel.SetActive(false);
    }
    
    public void OpenStageEntryPanel()
    {
        stageEntryPanel.SetActive(true);
    }

    public void OpenLaboratoryEntryPanel()
    {
        Debug.Log("연구실 판넬 켜기");
    }

    public void OpenBarEntryPanel()
    {
        Debug.Log("주점 판넬 켜기");
    }

    public void OpenForgeEntryPanel()
    {
        forgeEntryPanel.SetActive(true);
    }

    public void OpenMenuPanel()
    {
        menuPanel.SetActive(true);
    }
}
