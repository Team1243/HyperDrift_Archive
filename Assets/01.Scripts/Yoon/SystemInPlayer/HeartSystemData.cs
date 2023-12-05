using UnityEngine;
using System;

[Serializable]
public class HeartSystemData
{
    public int CurrentHeart = 0;
    public int MaxHeart = 20; // 하트의 최대 개수

    public int GenerateDelayTime = 5; // 생성 주기 15분

    public DateTime LastGenerateTime;
    public string LastGenerateTimeTxtData;

    public bool IsHeartMax => CurrentHeart >= MaxHeart;
    public bool IsHeartExist => CurrentHeart > 0;

    public void DataDebug()
    {
        Debug.Log($"CurrentHeart: {CurrentHeart}, MaxHeart: {MaxHeart}, LastGenerateTime: {LastGenerateTime.ToString()}");
    }
}