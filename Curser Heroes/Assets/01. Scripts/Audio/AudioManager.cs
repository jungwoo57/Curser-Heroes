using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitType { Monster, Cursor } // 열거형 

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioClip monsterHitClip, cursorHitClip;
    private AudioSource src;

    void Awake()
    {
        Instance = this;
        src = GetComponent<AudioSource>();
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