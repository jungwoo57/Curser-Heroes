using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleUI : MonoBehaviour
{
    public void OnClickGameStart()
    {
        SceneManager.LoadScene("JW_StageSelectUI");
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
