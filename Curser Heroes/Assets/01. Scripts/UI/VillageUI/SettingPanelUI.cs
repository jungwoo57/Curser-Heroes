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
        mainSlider.value = AudioListener.volume;
        if (AudioManager.Instance != null)
        {
            bgmSlider.value = AudioManager.Instance.bgmSource.volume;
            sfxSlider.value = AudioManager.Instance.src.volume;
        }
        mouseSensitivitySlider.value = GameManager.Instance.mouseSensitivity;
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
        Screen.SetResolution(1980, 1280, true);
        
    }

    public void OnClickWindowScreenButton()
    {
        Screen.SetResolution(1600, 900, false);
    }
}
