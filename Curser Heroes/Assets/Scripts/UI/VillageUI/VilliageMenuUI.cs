using UnityEngine;
using UnityEngine.SceneManagement;
public class VilliageMenuUI : MonoBehaviour
{
    public GameObject settingPanel;

    private void OnEnable()
    {
        CloseSettingPanel();
    }

    public void OpenSettingPanel()
    {
        settingPanel.SetActive(true);
    }

    public void CloseSettingPanel()
    {
        settingPanel.SetActive(false);
    }

    public void ClickBackButton()
    {
        gameObject.SetActive(false);
    }

    public void ClickTitleButton()
    {
        SceneManager.LoadScene("TitleSceneTest");
        Debug.Log("타이틀화면으로 돌아가기");   // 추후 타이틀씬으로 변경
    }
    
    
}
