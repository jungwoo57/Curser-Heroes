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
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBgm(bgmType.main);
        }
    }
    
    private void OnEnable()
    {
        Init();
    }

    private void Update()
    {
        if (!CheckEnablePanel())
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!menuPanel.activeSelf)
                {
                    menuPanel.SetActive(true);
                }
                else
                {
                    menuPanel.SetActive(false);
                }
            }
        }
    }

    private void Init()
    {
        stageEntryPanel.SetActive(false);
        laboratoryEntryPanel.SetActive(false);
        barEntryPanel.SetActive(false);
        forgeEntryPanel.SetActive(false);
        menuPanel.SetActive(false);
    }
    
    public void OpenStageEntryPanel()
    {
        stageEntryPanel.SetActive(true);
        if(AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonSound(buttonType.village);
    }

    public void OpenLaboratoryEntryPanel()
    {
        laboratoryEntryPanel.SetActive(true);
        if(AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonSound(buttonType.village);
    }

    public void OpenBarEntryPanel()
    {
        barEntryPanel.SetActive(true);
        if(AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonSound(buttonType.village);
    }

    public void OpenForgeEntryPanel()
    {
        forgeEntryPanel.SetActive(true);
        if(AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonSound(buttonType.village);
    }

    public void OpenMenuPanel()
    {
        menuPanel.SetActive(true);
    }

    private bool CheckEnablePanel()
    {
        if (stageEntryPanel.activeSelf)
            return true;
        if (laboratoryEntryPanel.activeSelf)
            return true;
        if (barEntryPanel.activeSelf)
            return true;
        if (forgeEntryPanel.activeSelf)
            return true;
        return false;
    }
}
