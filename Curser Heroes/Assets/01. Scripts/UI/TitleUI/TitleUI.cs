using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleUI : MonoBehaviour
{
    private void OnEnable()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBgm(bgmType.title);
        }
    }

    public void OnClickGameStart()
    {
        AudioManager.Instance.PlayButtonSound();
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
