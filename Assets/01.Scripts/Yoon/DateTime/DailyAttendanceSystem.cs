using UnityEngine;

public class DailyAttendanceSystem : MonoBehaviour
{
    public RewardInfoListSO dailyRewardInfoListSO;

    private int nowDayPassed = 0;
    public int NowDayPassed => nowDayPassed;
    private string saveKey = "NowDayPassed";

    private bool IsAWeekDone()
    {
        foreach (var d  in dailyRewardInfoListSO.RewardInfoList)
        {
            if (false == d.isRecieve)
            {
                return false;
            }
        }
        return true;
    }

    private void Awake()
    {
        // InitRewardDataAWeek();
        // UpdateDailyReward();
    }

    private void OnEnable()
    {
        nowDayPassed = PlayerPrefs.GetInt(saveKey);
        // Debug.LogError(nowDayPassed);

        RealDailyTimeSystem.OnDayHasPassed += UpdateDailyReward;
        DailyBonusPopup.OnUpdateInfo += OnRecieveReward;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt(saveKey, nowDayPassed);
        PlayerPrefs.Save();

        RealDailyTimeSystem.OnDayHasPassed -= UpdateDailyReward;
        DailyBonusPopup.OnUpdateInfo -= OnRecieveReward;
    }

    private void DayRecieveRewardCheck(int day)
    {
        dailyRewardInfoListSO.RewardInfoList[day].isRecieve = true;
    }

    private bool IsDayRecieveReward(int day)
    {
        return dailyRewardInfoListSO.RewardInfoList[day].isRecieve;
    }

    private void OnRecieveReward()
    {
        // if (nowDayPassed < 7)
        // {
        //     // Debug.LogError(NowDayPassed);
        //     nowDayPassed++;
        // }
        DayRecieveRewardCheck(nowDayPassed);
    }

    [ContextMenu("test")]
    private void UpdateDailyReward()
    {
        Debug.LogError("UpdateDailyReward");

        // 7일차까지 모두 보상을 받았다면
        if (IsAWeekDone())
        {
            InitRewardDataAWeek();
            return;
        }

        if (nowDayPassed < 6 && IsDayRecieveReward(nowDayPassed))
        {
            nowDayPassed++;
            Debug.LogError("UpdateDailyReward" + NowDayPassed);
        }
    }

    private void InitRewardDataAWeek()
    {
        // RewardInfoList에 데이터 비워주기
        dailyRewardInfoListSO.ResetList();

        // RewardInfoList에 데이터 넣어주기
        for (int i = 1; i < 8; i++)
        {
            RewardInfo rewardInfo = new RewardInfo();
            
            if (i % 2 == 0)
            {
                rewardInfo.RewardType = RewardType.Heart;
                rewardInfo.RewardAmount = i * 2;
            }
            else
            {
                rewardInfo.RewardType = RewardType.Coin;
                rewardInfo.RewardAmount = i * 1000;
            }

            dailyRewardInfoListSO.AddRewardInfo(rewardInfo);
        }

        nowDayPassed = 0;
        PlayerPrefs.DeleteKey(saveKey);
    }

}
