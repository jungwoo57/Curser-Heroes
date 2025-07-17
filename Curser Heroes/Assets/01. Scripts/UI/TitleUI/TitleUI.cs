using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleUI : MonoBehaviour
{
    public void OnClickGameStart()
    {
        TitleMainLoadManager.Instance.LoadMain();
    }

    public void OnClickExitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
    
    
}
