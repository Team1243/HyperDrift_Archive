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
    /// Ǯ���� Pop�� ��
    /// clip�� ���缭 Audio�� ����ǰ�, �ڵ����� Push�մϴ�.  
    /// </summary>
    /// <param name="clip">����� clip</param>
    /// <param name="volume">����� clip�� volume</param>
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
