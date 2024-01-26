using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RewardInfoData", menuName = "SO/Reward/RewardInfoListSO")]
public class RewardInfoListSO : ScriptableObject
{
    public List<RewardInfo> RewardInfoList = new List<RewardInfo>();    

    public void AddRewardInfo(RewardInfo rewardInfo)
    {
        RewardInfoList.Add(rewardInfo);
    }

    public void ResetInfoData()
    {
        foreach (RewardInfo info in RewardInfoList)
        {
            info.isRecieve = false;
        }
    }

    public void ResetList()
    {
        RewardInfoList.Clear();
    }
}
