using UnityEngine;

public enum HitType { Monster, Cursor } // 열거형 
public enum bgmType{main, battle, title}

public enum buttonType { title, village, upgrade}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [Header("오디오 클립")]
    public AudioClip monsterHitClip, cursorHitClip;
    public AudioClip mainBgm, battleBgm, titleBgm;
    public AudioClip titlebuttonClip;
    public AudioClip uibuttonClip;
    public AudioClip upgradeClip;
    [Header("오디오 소스")]
    public AudioSource bgmSource;  // 추가된 bgm 소스
    public AudioSource src;    // 기존 효과음 ()
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        AudioSource[] audioSources = gameObject.GetComponents<AudioSource>();
        src = audioSources[0];
        bgmSource = audioSources[1];
        PlayBgm(bgmType.title);
    }

    public void PlayBgm(bgmType type)
    {
        bgmSource.Stop();
        
        switch (type)
        {
            case bgmType.main: 
                bgmSource.clip = mainBgm;
                break;
            case bgmType.title:
                bgmSource.clip = titleBgm;
                break;
            case bgmType.battle: 
                bgmSource.clip = battleBgm;
                break;
        }
        bgmSource.loop = true;
        bgmSource.Play();
    }
    public void PlayHitSound(HitType type)
    {
        
        AudioClip clip;

        if (type == HitType.Monster)
        {
            clip = monsterHitClip;
        }
        else
        {
            clip = cursorHitClip;
        }

        src.PlayOneShot(clip);
    }

    public void PlayButtonSound(buttonType type)
    {
        AudioClip clip;

        switch (type)
        {
            case buttonType.title:
                clip = titlebuttonClip;
                src.PlayOneShot(clip);
                break;
            case buttonType.village:
                clip = uibuttonClip;
                src.PlayOneShot(clip);
                break;
            case buttonType.upgrade:
                clip = upgradeClip;
                src.PlayOneShot(clip);
                break;
        }
    }
}