using UnityEngine;

public enum HitType { Monster, Cursor } // 열거형 

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("오디오 클립")]
    public AudioClip monsterHitClip, cursorHitClip;
    public AudioClip mainBgm, battleBgm;
    
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
        PlayBgm(true);
    }

    public void PlayBgm(bool isBattle)
    {
        if (isBattle)
        {
            bgmSource.clip = battleBgm;
        }
        else
        {
            bgmSource.clip = mainBgm;
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
}