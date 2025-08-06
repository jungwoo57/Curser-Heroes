using UnityEngine;
using UnityEngine.UI;
public class SettingPanelUI : MonoBehaviour
{
    public Slider mainSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Slider mouseSensitivitySlider;
    public bool isFullScreen;
    
    void Start()
    {
        mainSlider.onValueChanged.AddListener(SetMainVolume);
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        mouseSensitivitySlider.onValueChanged.AddListener(SetMouseSensitivity);
    }

    void SetMainVolume(float value)
    {
        AudioListener.volume = value; // 전체 볼륨 조절
    }

    void SetBGMVolume(float value)
    {
        AudioManager.Instance.bgmSource.volume = value;
    }
    
    void SetSFXVolume(float value)
    {
        AudioManager.Instance.src.volume = value;
    }

    void SetMouseSensitivity(float value)
    {
        GameManager.Instance.mouseSensitivity = value;
    }
    public void OnClickExitButton()
    {
        gameObject.SetActive(false);
    }

    public void OnClickFullScreenButton()
    {
        Screen.fullScreen = true;
        Debug.Log(Screen.fullScreen);
    }

    public void OnClickWindowScreenButton()
    {
        Screen.fullScreen = false;
        Debug.Log(Screen.fullScreen);
    }
}
