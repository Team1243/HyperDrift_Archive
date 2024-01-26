using System;

public enum RewardType
{
    None,
    Heart,
    Coin
}

[Serializable]
public class RewardInfo
{
    public int ID;
    public RewardType RewardType = RewardType.None;
    public int RewardAmount = 0;
    public bool isRecieve = false;

    public RewardInfo() { }
    public RewardInfo(RewardType type, int amount)
    {
        RewardType = type;
        RewardAmount = amount;
    }
}
