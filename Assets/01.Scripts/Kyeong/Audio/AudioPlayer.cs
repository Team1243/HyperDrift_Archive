using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour, IPoolable
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Initialize()
    {
        _audioSource.volume = 1;
    }

    /// <summary>
    /// 풀에서 Pop된 후
    /// clip에 맞춰서 Audio가 재생되고, 자동으로 Push합니다.  
    /// </summary>
    /// <param name="clip">재생할 clip</param>
    /// <param name="volume">재생할 clip의 volume</param>
    public void AudioPlay(AudioClip clip, float volume)
    {
        _audioSource.clip = clip;
        _audioSource.volume = volume;
        _audioSource.Play();
        StartCoroutine(ClipPlayTime(clip.length + 0.1f));
    }

    private IEnumerator ClipPlayTime(float t)
    {
        yield return new WaitForSeconds(t);
        PoolManager.Instance.Push(this);
    }
}
