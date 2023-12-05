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
    /// Ǯ���� Pop�� ��
    /// clip�� ���缭 Audio�� ����ǰ�, �ڵ����� Push�մϴ�.
    /// </summary>
    /// <param name="clip">����� audio clip</param>
    /// <param name="volume">����� audio clip�� volume</param>
    public void AudioPlay(AudioClip clip, float volume)
    {
        AudioPlayer audioPlayer = PoolManager.Instance.Pop("AudioPlayer") as AudioPlayer;
        volume = Mathf.Clamp01(volume);
        audioPlayer.AudioPlay(clip, volume);
    }
}
