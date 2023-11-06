using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private ObjectPool objectPool;
    
    private AudioSource bgmAudioSource;
    public AudioClip bgmClip;

    [SerializeField] [Range(0f, 1f)] private float bgmVolume;
    [SerializeField] [Range(0f, 1f)] private float sfxVolume;

    private SoundSource soundSource;
    
    private void Awake()
    {
        instance = this;
        objectPool = GetComponent<ObjectPool>();
        bgmAudioSource = GetComponent<AudioSource>();
        bgmAudioSource.loop = true;
        bgmAudioSource.volume = bgmVolume;
    }

    private void Start()
    {
        ChangeBGMAudio(bgmClip);
    }

    public void ChangeBGMAudio(AudioClip clip)
    {
        bgmAudioSource.Stop();
        bgmAudioSource.clip = clip;
        bgmAudioSource.Play();
    }

    public void PlayClip(AudioClip clip)
    {
        GameObject obj = instance.objectPool.SpawnFromPool("SoundSource");
        obj.SetActive(true);
        soundSource = obj.GetComponent<SoundSource>();
        soundSource.Play(clip, sfxVolume);
    }

    public void StopClip()
    {
        soundSource.Disable();
    }
}
