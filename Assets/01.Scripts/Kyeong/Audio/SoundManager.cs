using System;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    public static SoundManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
            Debug.LogError("Multiple SoundManager is running");
        _instance = this;
    }

    /// <summary>
    /// 풀에서 Pop된 후
    /// clip에 맞춰서 Audio가 재생되고, 자동으로 Push합니다.
    /// </summary>
    /// <param name="clip">재생할 audio clip</param>
    /// <param name="volume">재생할 audio clip의 volume</param>
    public void AudioPlay(AudioClip clip, float volume)
    {
        AudioPlayer audioPlayer = PoolManager.Instance.Pop("AudioPlayer") as AudioPlayer;
        volume = Mathf.Clamp01(volume);
        audioPlayer.AudioPlay(clip, volume);
    }
}
